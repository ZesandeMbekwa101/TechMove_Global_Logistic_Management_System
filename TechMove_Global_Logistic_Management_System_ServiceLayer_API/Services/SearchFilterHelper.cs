using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers
{
    public class SearchFilterHelper
    {
        public IQueryable<ClientModel> FilterClients(
            IQueryable<ClientModel> query,
            FilterDto filter,
            string? region)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(c =>
                    c.Client_Name.Contains(filter.Search) ||
                    c.Contact_Person.Contains(filter.Search) ||
                    c.Email_Address.Contains(filter.Search) ||
                    c.Region.Contains(filter.Search) ||
                    c.Country.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                query = query.Where(c => c.Region == region);
            }

            return query;
        }
        public IQueryable<ContractModel> FilterContracts(
         IQueryable<ContractModel> query,
         FilterModel filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(c =>
                    c.Client.Client_Name.Contains(filter.Search) ||
                    c.Contract_Id.ToString().Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(c => c.Status == filter.Status);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(c => c.Start_Date >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(c => c.End_Date <= filter.ToDate.Value);
            }

            return query;
        }

        public IQueryable<ServiceRequestModel> FilterServiceRequests(
            IQueryable<ServiceRequestModel> query,
            FilterModel filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(s =>
                    s.Service_Description.Contains(filter.Search) ||
                    s.Service_Request_Id.ToString().Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(s => s.Status == filter.Status);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(s => s.Created_On >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(s => s.Created_On <= filter.ToDate.Value);
            }

            return query;
        }

        public IQueryable<AuditLogModel> FilterAuditLogs(
            IQueryable<AuditLogModel> query,
            FilterModel filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(a =>
                    a.Description.Contains(filter.Search) ||
                    a.Module.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(a => a.Status == filter.Status);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(a => a.Created_On >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(a => a.Created_On <= filter.ToDate.Value);
            }

            return query;
        }
    }
}