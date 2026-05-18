using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;
using TechMove_Global_Logistic_Management_System.Models;
using TechMove_Global_Logistic_Management_System.Services;
using TechMove_Global_Logistic_Management_System.ViewModel;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ContractController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly AuditLogHelper _auditHelper;
        private readonly SearchFilterHelper _searchHelper;
        private readonly FileValidationService _fileValidationService;

        public ContractController(ApplicationDbContext context, IWebHostEnvironment environment, AuditLogHelper auditHelper, SearchFilterHelper searchHelper, FileValidationService fileValidationService)
        {
            _context = context;
            _environment = environment;
            _auditHelper = auditHelper;
            _searchHelper = searchHelper;
            _fileValidationService = fileValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> ContractTabView(FilterViewModel filter)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.SupportDocument)
                .AsQueryable();

            query = _searchHelper.FilterContracts(query, filter);

            var contracts = await query
                .OrderByDescending(c => c.Contract_Id)
                .ToListAsync();

            ViewBag.Clients = await _context.Clients
                .OrderBy(c => c.Client_Name)
                .ToListAsync();

            return View(contracts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContract(ContractModel model, IFormFile? pdfFile)
        {
            if (!ModelState.IsValid)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Created",
                    "Contracts",
                    "Failed attempt to create a contract because required fields were missing.",
                    "Failed"
                );

                TempData["Error"] = "Please complete all required contract fields.";
                return RedirectToAction("ContractTabView");
            }

            if (pdfFile != null)
            {
                try
                {
                    _fileValidationService.ValidatePdfFile(pdfFile.FileName);
                }
                catch
                {
                    await _auditHelper.LogAsync(
                        model.Admin_Id,
                        "Uploaded",
                        "Contracts",
                        $"Failed PDF upload attempt for contract linked to client ID {model.Client_Id}. Only PDF files are allowed.",
                        "Failed"
                    );

                    TempData["Error"] = "Only PDF files are allowed.";
                    return RedirectToAction("ContractTabView");
                }

                var document = await SavePdfDocument(pdfFile);
                model.Support_Doc_Id = document.Support_Doc_Id;

                await _auditHelper.LogAsync(
                    model.Admin_Id,
                    "Uploaded",
                    "Files",
                    $"Uploaded signed agreement '{pdfFile.FileName}'."
                );
            }

            _context.Contracts.Add(model);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                model.Admin_Id,
                "Created",
                "Contracts",
                $"Created contract CT-{model.Contract_Id} for client ID {model.Client_Id}."
            );

            TempData["Success"] = "Contract added successfully.";
            return RedirectToAction("ContractTabView");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContract(ContractModel model, IFormFile? pdfFile)
        {
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Contract_Id == model.Contract_Id);

            if (contract == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Updated",
                    "Contracts",
                    $"Failed attempt to update contract CT-{model.Contract_Id} because it was not found.",
                    "Failed"
                );

                TempData["Error"] = "Contract not found.";
                return RedirectToAction("ContractTabView");
            }

            contract.Client_Id = model.Client_Id;
            contract.Admin_Id = model.Admin_Id;
            contract.Start_Date = model.Start_Date;
            contract.End_Date = model.End_Date;
            contract.Status = model.Status;
            contract.Service_Level = model.Service_Level;

            if (pdfFile != null)
            {
                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    await _auditHelper.LogAsync(
                        model.Admin_Id,
                        "Uploaded",
                        "Contracts",
                        $"Failed PDF replacement attempt for contract CT-{model.Contract_Id}. Only PDF files are allowed.",
                        "Failed"
                    );

                    TempData["Error"] = "Only PDF files are allowed.";
                    return RedirectToAction("ContractTabView");
                }

                var document = await SavePdfDocument(pdfFile);
                contract.Support_Doc_Id = document.Support_Doc_Id;

                await _auditHelper.LogAsync(
                    model.Admin_Id,
                    "Uploaded",
                    "Files",
                    $"Replaced signed agreement for contract CT-{model.Contract_Id} with '{pdfFile.FileName}'."
                );
            }

            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                model.Admin_Id,
                "Updated",
                "Contracts",
                $"Updated contract CT-{model.Contract_Id}."
            );

            TempData["Success"] = "Contract updated successfully.";
            return RedirectToAction("ContractTabView");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Contract_Id == id);

            if (contract == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Deleted",
                    "Contracts",
                    $"Failed attempt to delete contract CT-{id} because it was not found.",
                    "Failed"
                );

                TempData["Error"] = "Contract not found.";
                return RedirectToAction("ContractTabView");
            }

            if (contract.ServiceRequests.Any())
            {
                await _auditHelper.LogAsync(
                    contract.Admin_Id,
                    "Deleted",
                    "Contracts",
                    $"Failed attempt to delete contract CT-{id} because it has active service requests.",
                    "Failed"
                );

                TempData["Error"] = "Cannot delete this contract because it has service requests.";
                return RedirectToAction("ContractTabView");
            }

            var contractNumber = contract.Contract_Id;
            var adminId = contract.Admin_Id;

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                adminId,
                "Deleted",
                "Contracts",
                $"Deleted contract CT-{contractNumber}."
            );

            TempData["Success"] = "Contract deleted successfully.";
            return RedirectToAction("ContractTabView");
        }

        private async Task<SupportDocumentModel> SavePdfDocument(IFormFile pdfFile)
        {
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "contracts");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(pdfFile.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var document = new SupportDocumentModel
            {
                File_Name = pdfFile.FileName,
                File_Path = "/uploads/contracts/" + uniqueFileName,
                File_Type = ".pdf",
                Uploaded_On = DateTime.Now
            };

            _context.SupportDocuments.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }
    }
}