using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class AuditLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AuditLogTabView()
        {
            var logs = await _context.AuditLogs
                .Include(a => a.Admin)
                .OrderByDescending(a => a.Created_On)
                .ToListAsync();

            return View(logs);
        }
    }
}