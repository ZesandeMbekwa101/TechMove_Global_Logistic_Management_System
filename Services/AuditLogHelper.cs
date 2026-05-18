using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Models;

namespace TechMove_Global_Logistic_Management_System.Helpers
{
    public class AuditLogHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogHelper(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(
            int adminId,
            string action,
            string module,
            string description,
            string status = "Success")
        {
            var ipAddress = _httpContextAccessor
                .HttpContext?
                .Connection?
                .RemoteIpAddress?
                .ToString();

            var log = new AuditLogModel
            {
                Admin_Id = adminId,
                Action = action,
                Module = module,
                Description = description,
                IP_Address = ipAddress,
                Status = status,
                Created_On = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}