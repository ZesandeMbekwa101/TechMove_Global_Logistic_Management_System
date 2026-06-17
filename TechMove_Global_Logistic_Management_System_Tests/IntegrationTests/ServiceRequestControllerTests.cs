using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechMove_Global_Logistic_Management_System_Tests.IntegrationTests;

public class ServiceRequestControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ServiceRequestControllerTests(
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
                "/api/ServiceRequest");

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
                "/api/ServiceRequest/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsNotFound()
    {
        // Act
        var response =
            await _client.DeleteAsync(
                "/api/ServiceRequest/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_InvalidContract_ReturnsNotFound()
    {
        // Arrange
        var request = new
        {
            Contract_Id = 999999,
            Service_Description = "Container Shipping",
            ZAR_Amount = 5000,
            Target_Currency = "USD",
            Status = "Pending"
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/ServiceRequest",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_InvalidServiceRequest_ReturnsNotFound()
    {
        // Arrange
        var request = new
        {
            Service_Request_Id = 999999,
            Contract_Id = 1,
            Service_Description = "Updated Request",
            ZAR_Amount = 10000,
            Target_Currency = "USD",
            Status = "Approved"
        };

        // Act
        var response =
            await _client.PutAsJsonAsync(
                "/api/ServiceRequest/UpdateServiceRequest",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}