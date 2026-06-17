using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_Tests.UnitTests;

public class SearchFilterHelperTests
{
    private readonly SearchFilterHelper _helper;

    public SearchFilterHelperTests()
    {
        _helper = new SearchFilterHelper();
    }

    [Fact]
    public void FilterClients_SearchByClientName_ReturnsMatchingClient()
    {
        // Arrange
        var clients = new List<ClientModel>
        {
            new ClientModel
            {
                Client_Name = "TechMove",
                Contact_Person = "John",
                Email_Address = "john@test.com",
                Region = "Western Cape",
                Country = "South Africa"
            },
            new ClientModel
            {
                Client_Name = "Logistics Pro",
                Contact_Person = "Sarah",
                Email_Address = "sarah@test.com",
                Region = "Gauteng",
                Country = "South Africa"
            }
        }.AsQueryable();

        var filter = new FilterDto
        {
            Search = "TechMove"
        };

        // Act
        var result = _helper
            .FilterClients(clients, filter, null)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("TechMove", result[0].Client_Name);
    }

    [Fact]
    public void FilterClients_ByRegion_ReturnsCorrectClients()
    {
        // Arrange
        var clients = new List<ClientModel>
        {
            new ClientModel
            {
                Client_Name = "Client A",
                Region = "Western Cape"
            },
            new ClientModel
            {
                Client_Name = "Client B",
                Region = "Gauteng"
            }
        }.AsQueryable();

        var filter = new FilterDto();

        // Act
        var result = _helper
            .FilterClients(clients, filter, "Western Cape")
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Western Cape", result[0].Region);
    }

    [Fact]
    public void FilterClients_NoFilter_ReturnsAllClients()
    {
        // Arrange
        var clients = new List<ClientModel>
        {
            new ClientModel { Client_Name = "A" },
            new ClientModel { Client_Name = "B" }
        }.AsQueryable();

        // Act
        var result = _helper
            .FilterClients(
                clients,
                new FilterDto(),
                null)
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void FilterContracts_ByStatus_ReturnsActiveContracts()
    {
        // Arrange
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                Contract_Id = 1,
                Status = "Active",
                Client = new ClientModel
                {
                    Client_Name = "TechMove"
                }
            },
            new ContractModel
            {
                Contract_Id = 2,
                Status = "Expired",
                Client = new ClientModel
                {
                    Client_Name = "ABC"
                }
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Status = "Active"
        };

        // Act
        var result = _helper
            .FilterContracts(contracts, filter)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Active", result[0].Status);
    }

    [Fact]
    public void FilterContracts_ByClientSearch_ReturnsMatchingContract()
    {
        // Arrange
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                Contract_Id = 1,
                Status = "Active",
                Client = new ClientModel
                {
                    Client_Name = "TechMove"
                }
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Search = "TechMove"
        };

        // Act
        var result = _helper
            .FilterContracts(contracts, filter)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Contract_Id);
    }

    [Fact]
    public void FilterServiceRequests_ByStatus_ReturnsCorrectResults()
    {
        // Arrange
        var requests = new List<ServiceRequestModel>
        {
            new ServiceRequestModel
            {
                Service_Request_Id = 1,
                Service_Description = "Transport",
                Status = "Pending"
            },
            new ServiceRequestModel
            {
                Service_Request_Id = 2,
                Service_Description = "Storage",
                Status = "Approved"
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Status = "Pending"
        };

        // Act
        var result = _helper
            .FilterServiceRequests(requests, filter)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Pending", result[0].Status);
    }

    [Fact]
    public void FilterServiceRequests_Search_ReturnsMatchingRequest()
    {
        // Arrange
        var requests = new List<ServiceRequestModel>
        {
            new ServiceRequestModel
            {
                Service_Request_Id = 1,
                Service_Description = "International Transport"
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Search = "Transport"
        };

        // Act
        var result = _helper
            .FilterServiceRequests(requests, filter)
            .ToList();

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public void FilterAuditLogs_ByStatus_ReturnsMatchingLogs()
    {
        // Arrange
        var logs = new List<AuditLogModel>
        {
            new AuditLogModel
            {
                Status = "Success",
                Module = "Authentication",
                Description = "Login"
            },
            new AuditLogModel
            {
                Status = "Failed",
                Module = "Authentication",
                Description = "Failed Login"
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Status = "Failed"
        };

        // Act
        var result = _helper
            .FilterAuditLogs(logs, filter)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Failed", result[0].Status);
    }

    [Fact]
    public void FilterAuditLogs_Search_ReturnsMatchingLogs()
    {
        // Arrange
        var logs = new List<AuditLogModel>
        {
            new AuditLogModel
            {
                Status = "Success",
                Module = "Contracts",
                Description = "Contract Created"
            }
        }.AsQueryable();

        var filter = new FilterModel
        {
            Search = "Created"
        };

        // Act
        var result = _helper
            .FilterAuditLogs(logs, filter)
            .ToList();

        // Assert
        Assert.Single(result);
    }
}