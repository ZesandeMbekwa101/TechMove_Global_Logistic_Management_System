using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moq;
using RichardSzalay.MockHttp;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_Tests.UnitTests;

public class FastForexServiceTests
{
    [Fact]
    public async Task ConvertZarToCurrencyAsync_ValidResponse_ReturnsConvertedData()
    {
        // Arrange
        var json =
        """
        {
            "result":
            {
                "USD": 0.055
            }
        }
        """;

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json")
        };

        var handler = new MockHttpMessageHandler(response);

        var httpClient = new HttpClient(handler);

        var configuration = new Mock<IConfiguration>();

        configuration
            .Setup(x => x["FastForex:ApiKey"])
            .Returns("test-api-key");

        var service = new FastForexService(
            httpClient,
            configuration.Object);

        // Act
        var result =
            await service.ConvertZarToCurrencyAsync(
                1000m,
                "USD");

        // Assert
        Assert.NotNull(result);

        Assert.Equal("ZAR", result.Base);
        Assert.Equal("USD", result.Target);

        Assert.Equal(1000m, result.Amount);

        Assert.Equal(0.055m, result.Rate);

        Assert.Equal(55m, result.ConvertedAmount);
    }

    [Fact]
    public async Task ConvertZarToCurrencyAsync_ApiReturnsError_ThrowsException()
    {
        // Arrange
        var response =
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Invalid request")
            };

        var handler =
            new MockHttpMessageHandler(response);

        var httpClient =
            new HttpClient(handler);

        var configuration =
            new Mock<IConfiguration>();

        configuration
            .Setup(x => x["FastForex:ApiKey"])
            .Returns("test-api-key");

        var service =
            new FastForexService(
                httpClient,
                configuration.Object);

        // Act + Assert
        var exception =
            await Assert.ThrowsAsync<Exception>(() =>
                service.ConvertZarToCurrencyAsync(
                    1000m,
                    "USD"));

        Assert.Equal(
            "Invalid request",
            exception.Message);
    }

    [Fact]
    public async Task ConvertZarToCurrencyAsync_RoundsValuesCorrectly()
    {
        // Arrange
        var json =
        """
        {
            "result":
            {
                "USD": 0.055123456
            }
        }
        """;

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json")
        };

        var handler =
            new MockHttpMessageHandler(response);

        var httpClient =
            new HttpClient(handler);

        var configuration =
            new Mock<IConfiguration>();

        configuration
            .Setup(x => x["FastForex:ApiKey"])
            .Returns("test-api-key");

        var service =
            new FastForexService(
                httpClient,
                configuration.Object);

        // Act
        var result =
            await service.ConvertZarToCurrencyAsync(
                1000m,
                "USD");

        // Assert
        Assert.Equal(0.055123m, result.Rate);
        Assert.Equal(55.12m, result.ConvertedAmount);
    }
}