using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var loginSuccess = await _authService.LoginAsync(username, password, HttpContext);

            if (!loginSuccess)
            {
                TempData["Error"] = "Invalid username or password";
                return RedirectToAction("DashboardView", "Admin");
            }

            TempData["Success"] = "Login successful";
            return RedirectToAction("DashboardView", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);

            TempData["Success"] = "Logged out successfully.";
            return RedirectToAction("DashboardView", "Admin");
        }
    }
}