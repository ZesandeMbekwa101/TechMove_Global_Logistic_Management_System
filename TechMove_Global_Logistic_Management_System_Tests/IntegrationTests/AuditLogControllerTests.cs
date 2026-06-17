using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechMove_Global_Logistic_Management_System_Tests.IntegrationTests;

public class AuditLogControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuditLogControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/AuditLog");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_InvalidId_ReturnsNotFound()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/AuditLog/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ReturnsJsonContent()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/AuditLog");

        // Assert
        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadAsStringAsync();

        Assert.False(
            string.IsNullOrWhiteSpace(json));
    }
}