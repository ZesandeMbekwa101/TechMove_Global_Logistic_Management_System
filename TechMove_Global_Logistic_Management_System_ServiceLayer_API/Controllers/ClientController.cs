using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditLogHelper _auditHelper;
        private readonly SearchFilterHelper _searchHelper;

        public ClientController(
            ApplicationDbContext context,
            AuditLogHelper auditHelper,
            SearchFilterHelper searchHelper)
        {
            _context = context;
            _auditHelper = auditHelper;
            _searchHelper = searchHelper;
        }

        // GET: api/client
        [HttpGet("GetAllClients")]
        public async Task<ActionResult<List<ClientResponseDto>>> GetClients(
            [FromQuery] FilterDto filter,
            string? region)
        {
            var query = _context.Clients
                .Include(c => c.Contracts)
                .AsQueryable();

            query = _searchHelper.FilterClients(query, filter, region);

            var clients = await query
                .OrderByDescending(c => c.Client_Id)
                .Select(c => new ClientResponseDto
                {
                    Client_Id = c.Client_Id,
                    Client_Name = c.Client_Name,
                    Contact_Person = c.Contact_Person,
                    Phone_Number = c.Phone_Number,
                    Email_Address = c.Email_Address,
                    Region = c.Region,
                    Country = c.Country,
                    Admin_Id = c.Admin_Id
                })
                .ToListAsync();

            return Ok(clients);
        }

        // POST: api/client
        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClient(CreateClientDto dto)
        {
            var model = new ClientModel
            {
                Client_Name = dto.Client_Name,
                Contact_Person = dto.Contact_Person,
                Phone_Number = dto.Phone_Number,
                Email_Address = dto.Email_Address,
                Region = dto.Region,
                Country = dto.Country,
                Admin_Id = dto.Admin_Id
            };

            _context.Clients.Add(model);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                model.Admin_Id,
                "Created",
                "Clients",
                $"Created new client: {model.Client_Name}"
            );

            return Ok(new { message = "Client added successfully" });
        }

        // PUT: api/client
        [HttpPut("UpdateClient")]
        public async Task<IActionResult> UpdateClient(UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(dto.Client_Id);

            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            client.Client_Name = dto.Client_Name;
            client.Contact_Person = dto.Contact_Person;
            client.Phone_Number = dto.Phone_Number;
            client.Email_Address = dto.Email_Address;
            client.Region = dto.Region;
            client.Country = dto.Country;

            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                client.Admin_Id,
                "Updated",
                "Clients",
                $"Updated client '{client.Client_Name}'"
            );

            return Ok(new { message = "Client updated successfully" });
        }

        // DELETE: api/client/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.Client_Id == id);

            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            if (client.Contracts.Any())
            {
                return BadRequest(new { message = "Cannot delete client with active contracts" });
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                client.Admin_Id,
                "Delete",
                "Clients",
                $"Deleted client '{client.Client_Name}'"
            );

            return Ok(new { message = "Client deleted successfully" });
        }
    }
}