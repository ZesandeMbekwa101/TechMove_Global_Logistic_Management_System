using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechMove_Global_Logistic_Management_System_Tests.IntegrationTests;

public class ClientControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClientControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllClients_ReturnsOk()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/client/GetAllClients");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task AddClient_ReturnsOk()
    {
        // Arrange
        var request = new
        {
            Client_Name = $"Test Client {Guid.NewGuid()}",
            Contact_Person = "John Smith",
            Phone_Number = "0123456789",
            Email_Address = "john@test.com",
            Region = "Western Cape",
            Country = "South Africa",
            Admin_Id = 1
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/client/AddClient",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteClient_InvalidId_ReturnsNotFound()
    {
        // Act
        var response =
            await _client.DeleteAsync(
                "/api/client/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}