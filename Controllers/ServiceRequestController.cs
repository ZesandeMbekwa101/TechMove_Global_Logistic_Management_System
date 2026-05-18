using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;
using TechMove_Global_Logistic_Management_System.Models;
using TechMove_Global_Logistic_Management_System.Services;
using TechMove_Global_Logistic_Management_System.ViewModel;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditLogHelper _auditHelper;
        private readonly CurrencyService _currencyService;
        private readonly ContractBusinessService _contractBusinessService;

        public ServiceRequestController(ApplicationDbContext context, AuditLogHelper auditHelper, CurrencyService currencyService, ContractBusinessService contractBusinessService)
        {
            _context = context;
            _auditHelper = auditHelper;
            _currencyService = currencyService;
            _contractBusinessService = contractBusinessService;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceRequestTabView()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(AddServiceRequestViewModel model)
        {
            var contract = await _context.Contracts.FindAsync(model.Contract_Id);

            if (contract == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Created",
                    "Service Requests",
                    $"Failed attempt to create service request because contract CT-{model.Contract_Id} was not found.",
                    "Failed"
                );

                TempData["Error"] = "Contract not found.";
                return RedirectToAction("ServiceRequestTabView");
            }

            if (!_contractBusinessService.CanCreateServiceRequest(contract))
            {
                await _auditHelper.LogAsync(
                    contract.Admin_Id,
                    "Created",
                    "Service Requests",
                    $"Failed attempt to create service request for CT-{model.Contract_Id} because contract is not active.",
                    "Failed"
                );

                TempData["Error"] = "You can only create service requests for active contracts.";
                return RedirectToAction("ServiceRequestTabView");
            }

            var serviceRequest = new ServiceRequestModel
            {
                Contract_Id = model.Contract_Id,
                Service_Description = model.Service_Description,
                USD_Amount = model.Amount,
                Currency_Code = model.Currency_Code,
                Exchange_Rate = model.Exchange_Rate,
                ZAR_Amount = _currencyService.ConvertToZar(model.Amount, model.Exchange_Rate),
                Status = model.Status,
                Created_On = DateTime.Now
            };

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                contract.Admin_Id,
                "Created",
                "Service Requests",
                $"Created service request SR-{serviceRequest.Service_Request_Id} for contract CT-{model.Contract_Id}."
            );

            TempData["Success"] = "Service request added successfully.";
            return RedirectToAction("ServiceRequestTabView");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServiceRequest(UpdateServiceRequestViewModel model)
        {
            var request = await _context.ServiceRequests
                .FirstOrDefaultAsync(s => s.Service_Request_Id == model.Service_Request_Id);

            if (request == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Updated",
                    "Service Requests",
                    $"Failed attempt to update service request SR-{model.Service_Request_Id} because it was not found.",
                    "Failed"
                );

                TempData["Error"] = "Service request not found.";
                return RedirectToAction("ServiceRequestTabView");
            }

            var contract = await _context.Contracts.FindAsync(model.Contract_Id);

            if (contract == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Updated",
                    "Service Requests",
                    $"Failed attempt to update service request SR-{model.Service_Request_Id} because contract CT-{model.Contract_Id} was not found.",
                    "Failed"
                );

                TempData["Error"] = "Contract not found.";
                return RedirectToAction("ServiceRequestTabView");
            }

            request.Contract_Id = model.Contract_Id;
            request.Service_Description = model.Service_Description;
            request.USD_Amount = model.Amount;
            request.Currency_Code = model.Currency_Code;
            request.Exchange_Rate = model.Exchange_Rate;
            request.ZAR_Amount = model.ZAR_Amount;
            request.Status = model.Status;

            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                contract.Admin_Id,
                "Updated",
                "Service Requests",
                $"Updated service request SR-{request.Service_Request_Id}."
            );

            TempData["Success"] = "Service request updated successfully.";
            return RedirectToAction("ServiceRequestTabView");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.Service_Request_Id == id);

            if (request == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Deleted",
                    "Service Requests",
                    $"Failed attempt to delete service request SR-{id} because it was not found.",
                    "Failed"
                );

                TempData["Error"] = "Service request not found.";
                return RedirectToAction("ServiceRequestTabView");
            }

            var adminId = request.Contract?.Admin_Id ?? 1;

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                adminId,
                "Deleted",
                "Service Requests",
                $"Deleted service request SR-{id}."
            );

            TempData["Success"] = "Service request deleted successfully.";
            return RedirectToAction("ServiceRequestTabView");
        }
        [HttpGet]
        public async Task<IActionResult> ConvertToZar(decimal amount, string currency)
        {
            var apiKey = "71e249662e-ed86c316d0-tf8pmu";

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var url = $"https://api.fastforex.io/convert?from={currency}&to=ZAR&amount={amount}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Currency conversion failed.");
            }

            var json = await response.Content.ReadAsStringAsync();

            using var document = System.Text.Json.JsonDocument.Parse(json);

            var result = document.RootElement
                .GetProperty("result")
                .GetProperty("ZAR")
                .GetDecimal();

            var exchangeRate = result / amount;

            return Json(new
            {
                exchangeRate = exchangeRate.ToString("0.0000"),
                zarAmount = result.ToString("0.00")
            });
        }
    }
}