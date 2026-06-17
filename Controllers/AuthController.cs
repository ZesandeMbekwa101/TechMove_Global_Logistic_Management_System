using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Services;
using TechMove_Global_Logistic_Management_System.ViewModels;

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
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var success = await _authService.LoginAsync(dto, HttpContext);

            if (!success)
            {
                TempData["Error"] = "Invalid username or password";
                return RedirectToAction("LoginView");
            }

            TempData["Success"] = "Login successful";
            return RedirectToAction("DashboardView", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);

            TempData["Success"] = "Logged out successfully";
            return RedirectToAction("LoginView");
        }
    }
}