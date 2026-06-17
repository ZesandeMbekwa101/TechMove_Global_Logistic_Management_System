using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class AuditLogController : Controller
    {
        private readonly AuditLogService _auditLogService;

        public AuditLogController(
            AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult>
            AuditLogTabView()
        {
            var auditLogs =
                await _auditLogService
                .GetAllAuditLogsAsync();

            return View(auditLogs);
        }
    }
}