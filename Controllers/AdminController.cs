using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuditLogHelper _auditHelper;
        private readonly ApplicationDbContext _context;

        public AdminController(AuditLogHelper auditHelper, ApplicationDbContext context)
        {
            _auditHelper = auditHelper;
            _context = context;
        }
        public IActionResult DashboardView()
        {
            return View();
        }
    }
}
