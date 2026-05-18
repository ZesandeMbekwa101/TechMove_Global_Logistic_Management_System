using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;
using TechMove_Global_Logistic_Management_System.Models;
using TechMove_Global_Logistic_Management_System.ViewModel;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ClientController : Controller
    {
        private readonly AuditLogHelper _auditHelper;
        private readonly ApplicationDbContext _context;
        private readonly SearchFilterHelper _searchHelper;

        public ClientController(ApplicationDbContext context, AuditLogHelper auditHelper, SearchFilterHelper searchHelper)
        {
            _context = context;
            _auditHelper = auditHelper;
            _searchHelper = searchHelper;
        }
        [HttpGet]
        public async Task<IActionResult> ClientTabView(FilterViewModel filter, string? region)
        {
            var query = _context.Clients
                .Include(c => c.Contracts)
                .AsQueryable();

            query = _searchHelper.FilterClients(query, filter, region);

            var clients = await query
                .OrderByDescending(c => c.Client_Id)
                .ToListAsync();

            return View(clients);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientModel model)
        {
            if (!ModelState.IsValid)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Created",
                    "Clients",
                    "Failed attempt to add a new client because required fields were missing.",
                    "Failed"
                );

                TempData["Error"] = "Please complete all required fields.";
                return RedirectToAction("ClientTabView");
            }

            _context.Clients.Add(model);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                model.Admin_Id,
                "Created",
                "Clients",
                $"Created new client: {model.Client_Name}"
            );

            TempData["Success"] = "Client added successfully.";
            return RedirectToAction("ClientTabView");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient(ClientModel model)
        {
            var client = await _context.Clients.FindAsync(model.Client_Id);

            if (client == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Updated",
                    "Clients",
                    $"Failed attempt to update client with ID {model.Client_Id} because the client was not found.",
                    "Failed"
                );

                TempData["Error"] = "Client not found.";
                return RedirectToAction("ClientTabView");
            }

            var adminId = client.Admin_Id;

            client.Client_Name = model.Client_Name;
            client.Contact_Person = model.Contact_Person;
            client.Phone_Number = model.Phone_Number;
            client.Email_Address = model.Email_Address;
            client.Region = model.Region;
            client.Country = model.Country;

            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                adminId,
                "Updated",
                "Clients",
                $"Updated client '{client.Client_Name}'."
            );

            TempData["Success"] = "Client updated successfully.";
            return RedirectToAction("ClientTabView");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.Client_Id == id);

            if (client == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Delete",
                    "Clients",
                    $"Failed attempt to delete client with ID {id} because the client was not found.",
                    "Failed"
                );

                TempData["Error"] = "Client not found.";
                return RedirectToAction("ClientTabView");
            }

            if (client.Contracts.Any())
            {
                await _auditHelper.LogAsync(
                    client.Admin_Id,
                    "Delete",
                    "Clients",
                    $"Failed attempt to delete client '{client.Client_Name}' because the client has active contracts.",
                    "Failed"
                );

                TempData["Error"] = "Cannot delete this client because it has contracts.";
                return RedirectToAction("ClientTabView");
            }

            var clientName = client.Client_Name;
            var adminId = client.Admin_Id;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                adminId,
                "Delete",
                "Clients",
                $"Deleted client '{clientName}'."
            );

            TempData["Success"] = "Client deleted successfully.";
            return RedirectToAction("ClientTabView");
        }
    }
}