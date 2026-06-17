using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL LOGS
        // =========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAll()
        {
            var logs = await _context.AuditLogs
                .Include(a => a.Admin)
                .OrderByDescending(a => a.Created_On)
                .Select(a => new AuditLogDto
                {
                    Audit_Log_Id = a.Audit_Log_Id,
                    Admin_Id = a.Admin_Id,
                    Admin_Name = a.Admin != null
                        ? a.Admin.First_Name + " " + a.Admin.Last_Name
                        : "Unknown",
                    Action = a.Action,
                    Module = a.Module,
                    Description = a.Description,
                    IP_Address = a.IP_Address,
                    Status = a.Status,
                    Created_On = a.Created_On
                })
                .ToListAsync();

            return Ok(logs);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLogDto>> GetById(int id)
        {
            var a = await _context.AuditLogs
                .Include(x => x.Admin)
                .FirstOrDefaultAsync(x => x.Audit_Log_Id == id);

            if (a == null)
                return NotFound(new { message = "Audit log not found." });

            return Ok(new AuditLogDto
            {
                Audit_Log_Id = a.Audit_Log_Id,
                Admin_Id = a.Admin_Id,
                Admin_Name = a.Admin != null
                    ? a.Admin.First_Name + " " + a.Admin.Last_Name
                    : "Unknown",
                Action = a.Action,
                Module = a.Module,
                Description = a.Description,
                IP_Address = a.IP_Address,
                Status = a.Status,
                Created_On = a.Created_On
            });
        }
    }
}