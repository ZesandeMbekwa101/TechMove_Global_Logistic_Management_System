using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Models.DTOs;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ContractController : Controller
    {
        private readonly ContractService _contractService;
        private readonly ClientService _clientService;

        public ContractController(
            ContractService contractService,
            ClientService clientService)
        {
            _contractService = contractService;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> ContractTabView()
        {
            var contracts =
                await _contractService.GetAllContractsAsync();

            ViewBag.Clients =
                await _clientService.GetAllClientsAsync();

            return View(contracts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContract(CreateContractDto dto)
        {
            var success =
                await _contractService.AddContractAsync(dto);

            TempData[success ? "Success" : "Error"] =
                success
                ? "Contract added successfully"
                : "Failed to add contract";

            return RedirectToAction(nameof(ContractTabView));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContract(UpdateContractDto dto)
        {
            var success =
                await _contractService.UpdateContractAsync(dto);

            TempData[success ? "Success" : "Error"] =
                success
                ? "Contract updated successfully"
                : "Failed to update contract";

            return RedirectToAction(nameof(ContractTabView));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var success =
                await _contractService.DeleteContractAsync(id);

            TempData[success ? "Success" : "Error"] =
                success
                ? "Contract deleted successfully"
                : "Failed to delete contract";

            return RedirectToAction(nameof(ContractTabView));
        }
    }
}