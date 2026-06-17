using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using Xunit;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Tests
{
    public class AuditLogHelperTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private IHttpContextAccessor GetHttpContextAccessor()
        {
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            return new HttpContextAccessor
            {
                HttpContext = context
            };
        }

        [Fact]
        public async Task LogAsync_ShouldSaveAuditLogToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            var helper = new AuditLogHelper(context, httpContextAccessor);

            // Act
            await helper.LogAsync(
                1,
                "Created",
                "Clients",
                "Created new client DHL",
                "Success"
            );

            // Assert
            var log = await context.AuditLogs.FirstOrDefaultAsync();

            Assert.NotNull(log);
            Assert.Equal(1, log.Admin_Id);
            Assert.Equal("Created", log.Action);
            Assert.Equal("Clients", log.Module);
            Assert.Equal("Created new client DHL", log.Description);
            Assert.Equal("Success", log.Status);
            Assert.Equal("127.0.0.1", log.IP_Address);
        }

        [Fact]
        public async Task LogAsync_WhenStatusNotProvided_ShouldDefaultToSuccess()
        {
            // Arrange
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            var helper = new AuditLogHelper(context, httpContextAccessor);

            // Act
            await helper.LogAsync(
                1,
                "Login",
                "Authentication",
                "Admin logged in"
            );

            // Assert
            var log = await context.AuditLogs.FirstOrDefaultAsync();

            Assert.NotNull(log);
            Assert.Equal("Success", log.Status);
        }

        [Fact]
        public async Task LogAsync_ShouldSaveFailedAuditLog()
        {
            // Arrange
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            var helper = new AuditLogHelper(context, httpContextAccessor);

            // Act
            await helper.LogAsync(
                1,
                "Login",
                "Authentication",
                "Failed login attempt",
                "Failed"
            );

            // Assert
            var log = await context.AuditLogs.FirstOrDefaultAsync();

            Assert.NotNull(log);
            Assert.Equal("Failed", log.Status);
            Assert.Equal("Failed login attempt", log.Description);
        }
    }
}