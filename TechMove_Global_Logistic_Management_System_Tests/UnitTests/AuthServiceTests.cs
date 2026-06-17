using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_Tests.UnitTests;

public class AuthServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private JwtService GetJwtService()
    {
        var settings = new Dictionary<string, string>
        {
            {"JwtSettings:Key", "ThisIsATestKeyThatIsLongEnough123456"},
            {"JwtSettings:Issuer", "TechMove"},
            {"JwtSettings:Audience", "TechMoveUsers"},
            {"JwtSettings:DurationInMinutes", "60"}
        };

        IConfiguration configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(settings!)
                .Build();

        return new JwtService(configuration);
    }

    private AuditLogHelper GetAuditHelper(
        ApplicationDbContext context)
    {
        var accessor = new Mock<IHttpContextAccessor>();

        accessor.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        return new AuditLogHelper(
            context,
            accessor.Object);
    }

    [Fact]
    public async Task RegisterAsync_NewUser_ReturnsAdmin()
    {
        // Arrange
        var context = GetDbContext();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.RegisterAsync(
            "John",
            "Smith",
            "johnsmith",
            "Password123");

        // Assert
        Assert.NotNull(result);

        Assert.Equal("John", result.First_Name);
        Assert.Equal("Smith", result.Last_Name);
        Assert.Equal("johnsmith", result.Username);

        Assert.False(
            string.IsNullOrWhiteSpace(
                result.Password_Hash));
    }

    [Fact]
    public async Task RegisterAsync_ExistingUser_ReturnsNull()
    {
        // Arrange
        var context = GetDbContext();

        context.Admins.Add(new AdminModel
        {
            Username = "admin",
            Password_Hash = "hashed"
        });

        await context.SaveChangesAsync();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.RegisterAsync(
            "John",
            "Smith",
            "admin",
            "Password123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_HashesPassword()
    {
        // Arrange
        var context = GetDbContext();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.RegisterAsync(
            "John",
            "Smith",
            "johnsmith",
            "Password123");

        // Assert
        Assert.NotEqual(
            "Password123",
            result!.Password_Hash);

        Assert.True(
            BCrypt.Net.BCrypt.Verify(
                "Password123",
                result.Password_Hash));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var context = GetDbContext();

        var admin = new AdminModel
        {
            First_Name = "John",
            Last_Name = "Smith",
            Username = "admin",
            Password_Hash =
                BCrypt.Net.BCrypt.HashPassword(
                    "Password123")
        };

        context.Admins.Add(admin);

        await context.SaveChangesAsync();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.LoginAsync(
            "admin",
            "Password123");

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            "admin",
            result.Value.admin.Username);

        Assert.False(
            string.IsNullOrWhiteSpace(
                result.Value.token));
    }

    [Fact]
    public async Task LoginAsync_InvalidUsername_ReturnsNull()
    {
        // Arrange
        var context = GetDbContext();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.LoginAsync(
            "unknown",
            "Password123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var context = GetDbContext();

        context.Admins.Add(new AdminModel
        {
            Username = "admin",
            Password_Hash =
                BCrypt.Net.BCrypt.HashPassword(
                    "CorrectPassword")
        });

        await context.SaveChangesAsync();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.LoginAsync(
            "admin",
            "WrongPassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_GeneratesJwtToken()
    {
        // Arrange
        var context = GetDbContext();

        context.Admins.Add(new AdminModel
        {
            First_Name = "John",
            Last_Name = "Smith",
            Username = "admin",
            Password_Hash =
                BCrypt.Net.BCrypt.HashPassword(
                    "Password123")
        });

        await context.SaveChangesAsync();

        var service = new AuthService(
            context,
            GetJwtService(),
            GetAuditHelper(context));

        // Act
        var result = await service.LoginAsync(
            "admin",
            "Password123");

        // Assert
        Assert.NotNull(result);

        Assert.Contains(".",
            result.Value.token);

        Assert.True(
            result.Value.token.Split('.').Length == 3);
    }
}