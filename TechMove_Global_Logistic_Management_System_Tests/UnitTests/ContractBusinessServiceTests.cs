using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_Tests.UnitTests;

public class ContractBusinessServiceTests
{
    private readonly ContractBusinessService _service;

    public ContractBusinessServiceTests()
    {
        _service = new ContractBusinessService();
    }

    [Fact]
    public void CanCreateServiceRequest_ActiveContract_ReturnsTrue()
    {
        var contract = new ContractModel
        {
            Status = "Active"
        };

        var result = _service.CanCreateServiceRequest(contract);

        Assert.True(result);
    }

    [Fact]
    public void CanCreateServiceRequest_InactiveContract_ReturnsFalse()
    {
        var contract = new ContractModel
        {
            Status = "Expired"
        };

        var result = _service.CanCreateServiceRequest(contract);

        Assert.False(result);
    }

    [Fact]
    public void CanCreateServiceRequest_NullContract_ReturnsFalse()
    {
        var result = _service.CanCreateServiceRequest(null);

        Assert.False(result);
    }
}