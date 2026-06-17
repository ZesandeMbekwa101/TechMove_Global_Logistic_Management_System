using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_Tests.UnitTests;

public class CurrencyServiceTests
{
    private readonly CurrencyService _service;

    public CurrencyServiceTests()
    {
        _service = new CurrencyService();
    }

    [Fact]
    public void ConvertToZar_ValidValues_ReturnsCorrectAmount()
    {
        var result = _service.ConvertToZar(100, 18.50m);

        Assert.Equal(1850m, result);
    }

    [Fact]
    public void ConvertToZar_NegativeAmount_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            _service.ConvertToZar(-100, 18));
    }

    [Fact]
    public void ConvertToZar_InvalidExchangeRate_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            _service.ConvertToZar(100, 0));
    }

    [Fact]
    public void CalculateExchangeRate_ReturnsCorrectRate()
    {
        var result = _service.CalculateExchangeRate(1850m, 100m);

        Assert.Equal(18.5m, result);
    }

    [Fact]
    public void CalculateExchangeRate_InvalidOriginalAmount_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            _service.CalculateExchangeRate(1850m, 0));
    }
}