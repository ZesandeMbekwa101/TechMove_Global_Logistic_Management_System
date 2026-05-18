using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditLogHelper _auditHelper;

        public AuthService(ApplicationDbContext context, AuditLogHelper auditHelper)
        {
            _context = context;
            _auditHelper = auditHelper;
        }

        public async Task<bool> LoginAsync(string username, string password, HttpContext httpContext)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a =>
                    a.Username == username &&
                    a.Password_Hash == password);

            if (admin == null)
            {
                await _auditHelper.LogAsync(
                    1,
                    "Login",
                    "Authentication",
                    $"Failed login attempt for username '{username}'",
                    "Failed"
                );

                return false;
            }

            httpContext.Session.SetInt32("AdminId", admin.Admin_Id);
            httpContext.Session.SetString("AdminUsername", admin.Username);

            await _auditHelper.LogAsync(
                admin.Admin_Id,
                "Login",
                "Authentication",
                $"{admin.Username} logged into the admin dashboard"
            );

            return true;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            var adminId = httpContext.Session.GetInt32("AdminId");
            var username = httpContext.Session.GetString("AdminUsername");

            if (adminId != null)
            {
                await _auditHelper.LogAsync(
                    adminId.Value,
                    "Logout",
                    "Authentication",
                    $"{username} logged out of the admin dashboard"
                );
            }

            httpContext.Session.Clear();
        }
    }
}