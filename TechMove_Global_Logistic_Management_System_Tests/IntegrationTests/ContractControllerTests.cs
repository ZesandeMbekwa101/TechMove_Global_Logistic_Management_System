using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechMove_Global_Logistic_Management_System_Tests.IntegrationTests;

public class ContractControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContractControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllContracts_ReturnsOk()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/contract/GetAllContracts");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
    [Fact]
    public async Task DeleteContract_InvalidId_ReturnsNotFound()
    {
        // Act
        var response =
            await _client.DeleteAsync(
                "/api/contract/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
    [Fact]
    public async Task AddContract_ReturnsOk()
    {
        // Arrange
        var formData =
            new MultipartFormDataContent();

        formData.Add(
            new StringContent("1"),
            "Client_Id");

        formData.Add(
            new StringContent("1"),
            "Admin_Id");

        formData.Add(
            new StringContent(DateTime.Now.ToString("o")),
            "Start_Date");

        formData.Add(
            new StringContent(DateTime.Now.AddMonths(12).ToString("o")),
            "End_Date");

        formData.Add(
            new StringContent("Premium"),
            "Service_Level");

        formData.Add(
            new StringContent("Active"),
            "Status");

        // Act
        var response =
            await _client.PostAsync(
                "/api/contract/AddContract",
                formData);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
    [Fact]
    public async Task UpdateContract_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var formData =
            new MultipartFormDataContent();

        formData.Add(
            new StringContent("999999"),
            "Contract_Id");

        formData.Add(
            new StringContent("1"),
            "Client_Id");

        formData.Add(
            new StringContent("1"),
            "Admin_Id");

        formData.Add(
            new StringContent(DateTime.Now.ToString("o")),
            "Start_Date");

        formData.Add(
            new StringContent(DateTime.Now.AddMonths(12).ToString("o")),
            "End_Date");

        formData.Add(
            new StringContent("Premium"),
            "Service_Level");

        formData.Add(
            new StringContent("Active"),
            "Status");

        // Act
        var response =
            await _client.PutAsync(
                "/api/contract/UpdateContract",
                formData);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}