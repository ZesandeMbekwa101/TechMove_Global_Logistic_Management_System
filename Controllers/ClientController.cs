using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Models.DTOs;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> ClientTabView(
            string? search,
            string? region)
        {
            var clients =
                await _clientService.GetAllClientsAsync(
                    search,
                    region);

            return View(clients);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(CreateClientDto dto)
        {
            var success =
                await _clientService.AddClientAsync(dto);

            if (success)
                TempData["Success"] = "Client added successfully";
            else
                TempData["Error"] = "Failed to add client";

            return RedirectToAction(nameof(ClientTabView));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient(UpdateClientDto dto)
        {
            var success =
                await _clientService.UpdateClientAsync(dto);

            if (success)
                TempData["Success"] = "Client updated successfully";
            else
                TempData["Error"] = "Failed to update client";

            return RedirectToAction(nameof(ClientTabView));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var success =
                await _clientService.DeleteClientAsync(id);

            if (success)
                TempData["Success"] = "Client deleted successfully";
            else
                TempData["Error"] = "Failed to delete client";

            return RedirectToAction(nameof(ClientTabView));
        }
    }
}