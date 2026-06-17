using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechMove_Global_Logistic_Management_System_Tests.IntegrationTests;

public class AuthControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Logout_ReturnsOk()
    {
        // Act
        var response =
            await _client.PostAsync(
                "/api/auth/logout",
                null);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var request = new
        {
            Username = "invaliduser",
            Password = "invalidpassword"
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/auth/login",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Register_WithNewUser_ReturnsSuccess()
    {
        // Arrange
        var request = new
        {
            FirstName = "John",
            LastName = "Smith",
            Username = $"john{Guid.NewGuid()}",
            Password = "Password123"
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/auth/register",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Assert.False(
            string.IsNullOrWhiteSpace(content));
    }

    [Fact]
    public async Task Register_DuplicateUsername_ReturnsBadRequest()
    {
        // Arrange
        var username =
            $"duplicate{Guid.NewGuid()}";

        var request = new
        {
            FirstName = "John",
            LastName = "Smith",
            Username = username,
            Password = "Password123"
        };

        // First registration
        await _client.PostAsJsonAsync(
            "/api/auth/register",
            request);

        // Second registration
        var secondResponse =
            await _client.PostAsJsonAsync(
                "/api/auth/register",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task RegisterThenLogin_ReturnsToken()
    {
        // Arrange
        var username =
            $"user{Guid.NewGuid()}";

        var registerRequest = new
        {
            FirstName = "John",
            LastName = "Smith",
            Username = username,
            Password = "Password123"
        };

        // Register user
        await _client.PostAsJsonAsync(
            "/api/auth/register",
            registerRequest);

        var loginRequest = new
        {
            Username = username,
            Password = "Password123"
        };

        // Act
        var loginResponse =
            await _client.PostAsJsonAsync(
                "/api/auth/login",
                loginRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            loginResponse.StatusCode);

        var json =
            await loginResponse.Content.ReadAsStringAsync();

        Assert.Contains("token",
            json,
            StringComparison.OrdinalIgnoreCase);
    }
}