using Microsoft.AspNetCore.Mvc;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {
           
        }
        public IActionResult DashboardView()
        {
            return View();
        }
    }
}
