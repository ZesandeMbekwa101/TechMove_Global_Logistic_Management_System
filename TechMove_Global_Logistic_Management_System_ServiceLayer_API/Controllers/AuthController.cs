using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register(
    [FromBody] RegisterRequestDto request)
        {
            var admin = await _authService.RegisterAsync(
                request.FirstName,
                request.LastName,
                request.Username,
                request.Password);

            if (admin == null)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Message = "Username already exists"
                });
            }

            return Ok(new RegisterResponseDto
            {
                Success = true,
                Message = "Admin registered successfully",
                AdminId = admin.Admin_Id,
                Username = admin.Username
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(
                request.Username,
                request.Password);

            if (result == null)
            {
                return Unauthorized(new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }

            return Ok(new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                AdminId = result.Value.admin.Admin_Id,
                Username = result.Value.admin.Username,
                Token = result.Value.token
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT logout is handled client-side (delete token)
            return Ok(new LogoutResponseDto
            {
                Success = true,
                Message = "Logged out successfully"
            });
        }
    }
}