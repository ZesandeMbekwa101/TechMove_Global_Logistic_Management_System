using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly AuditLogHelper _auditHelper;

        public AuthService(
            ApplicationDbContext context,
            JwtService jwtService,
            AuditLogHelper auditHelper)
        {
            _context = context;
            _jwtService = jwtService;
            _auditHelper = auditHelper;
        }
        public async Task<AdminModel?> RegisterAsync(
    string firstName,
    string lastName,
    string username,
    string password)
        {
            // check if user exists
            var existingUser = await _context.Admins
                .FirstOrDefaultAsync(x => x.Username == username);

            if (existingUser != null)
                return null;

            // hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var admin = new AdminModel
            {
                First_Name = firstName,
                Last_Name = lastName,
                Username = username,
                Password_Hash = passwordHash,
                Created_On = DateTime.UtcNow
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            await _auditHelper.LogAsync(
                admin.Admin_Id,
                "Register",
                "Authentication",
                $"{username} registered a new admin account"
            );

            return admin;
        }

        public async Task<(AdminModel admin, string token)?> LoginAsync(
            string username,
            string password)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == username);

            if (admin == null)
            {
                await _auditHelper.LogAsync(
                    0,
                    "Login",
                    "Authentication",
                    $"Failed login attempt for username '{username}'",
                    "Failed"
                );

                return null;
            }

            // FIX: use hashed password check
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, admin.Password_Hash);

            if (!validPassword)
            {
                await _auditHelper.LogAsync(
                    admin.Admin_Id,
                    "Login",
                    "Authentication",
                    $"Failed login attempt for username '{username}'",
                    "Failed"
                );

                return null;
            }

            var token = _jwtService.GenerateToken(admin);

            await _auditHelper.LogAsync(
                admin.Admin_Id,
                "Login",
                "Authentication",
                $"{admin.Username} logged into the system"
            );

            return (admin, token);
        }
    }
}