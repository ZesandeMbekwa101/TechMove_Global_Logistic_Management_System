using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly AuditLogHelper _auditHelper;
        private readonly SearchFilterHelper _searchHelper;
        private readonly FileValidationService _fileValidationService;

        public ContractController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            AuditLogHelper auditHelper,
            SearchFilterHelper searchHelper,
            FileValidationService fileValidationService)
        {
            _context = context;
            _environment = environment;
            _auditHelper = auditHelper;
            _searchHelper = searchHelper;
            _fileValidationService = fileValidationService;
        }

        // GET: api/contract
        [HttpGet("GetAllContracts")]
        public async Task<ActionResult<List<ContractDto>>> GetContracts([FromQuery] FilterModel filter)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.SupportDocument)
                .AsQueryable();

            query = _searchHelper.FilterContracts(query, filter);

            var contracts = await query
                .OrderByDescending(c => c.Contract_Id)
                .Select(c => new ContractDto
                {
                    Contract_Id = c.Contract_Id,
                    Client_Id = c.Client_Id,
                    Admin_Id = c.Admin_Id,
                    Support_Doc_Id = c.Support_Doc_Id,
                    Start_Date = c.Start_Date,
                    End_Date = c.End_Date,
                    Status = c.Status,
                    Service_Level = c.Service_Level,
                    File_Name = c.SupportDocument != null ? c.SupportDocument.File_Name : null,
                    File_Path = c.SupportDocument != null ? c.SupportDocument.File_Path : null
                })
                .ToListAsync();

            return Ok(contracts);
        }

        // POST: api/contract
        [HttpPost("AddContract")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddContract([FromForm] CreateContractDto dto)
        {
            var contract = new ContractModel
            {
                Client_Id = dto.Client_Id,
                Admin_Id = dto.Admin_Id,
                Start_Date = dto.Start_Date,
                End_Date = dto.End_Date,
                Service_Level = dto.Service_Level,
                Status = dto.Status
            };

            if (dto.pdfFile != null)
            {
                _fileValidationService.ValidatePdfFile(dto.pdfFile.FileName);

                var document = await SavePdfDocument(dto.pdfFile);
                contract.Support_Doc_Id = document.Support_Doc_Id;
            }

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                contract.Admin_Id,
                "Created",
                "Contracts",
                $"Created contract CT-{contract.Contract_Id}"
            );

            return Ok(new { message = "Contract created successfully" });
        }

        // PUT: api/contract
        [HttpPut("UpdateContract")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateContract([FromForm] UpdateContractDto dto)
        {
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Contract_Id == dto.Contract_Id);

            if (contract == null)
                return NotFound(new { message = "Contract not found" });

            contract.Client_Id = dto.Client_Id;
            contract.Admin_Id = dto.Admin_Id;
            contract.Start_Date = dto.Start_Date;
            contract.End_Date = dto.End_Date;
            contract.Status = dto.Status;
            contract.Service_Level = dto.Service_Level;

            if (dto.pdfFile != null)
            {
                _fileValidationService.ValidatePdfFile(dto.pdfFile.FileName);

                var document = await SavePdfDocument(dto.pdfFile);
                contract.Support_Doc_Id = document.Support_Doc_Id;
            }

            // MISSING
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                contract.Admin_Id,
                "Updated",
                "Contracts",
                $"Updated contract CT-{contract.Contract_Id}"
            );

            return Ok(new { message = "Contract updated successfully" });
        }

        // DELETE: api/contract/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Contract_Id == id);

            if (contract == null)
                return NotFound(new { message = "Contract not found" });

            if (contract.ServiceRequests.Any())
                return BadRequest(new { message = "Cannot delete contract with service requests" });

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                contract.Admin_Id,
                "Deleted",
                "Contracts",
                $"Deleted contract CT-{id}"
            );

            return Ok(new { message = "Contract deleted successfully" });
        }

        // FILE SAVE METHOD
        private async Task<SupportDocumentModel> SavePdfDocument(IFormFile pdfFile)
        {
            // Use wwwroot if available, otherwise create it under ContentRoot
            var rootPath = _environment.WebRootPath;

            if (string.IsNullOrEmpty(rootPath))
            {
                rootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
            }

            var uploadFolder = Path.Combine(rootPath, "uploads", "contracts");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var document = new SupportDocumentModel
            {
                File_Name = pdfFile.FileName,
                File_Path = $"/uploads/contracts/{uniqueFileName}",
                File_Type = ".pdf",
                Uploaded_On = DateTime.Now
            };

            _context.SupportDocuments.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }
    }
}