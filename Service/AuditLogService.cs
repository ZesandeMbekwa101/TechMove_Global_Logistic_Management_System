using System.Net.Http.Json;
using TechMove_Global_Logistic_Management_System.Models.DTOs;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class AuditLogService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuditLogService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            _configuration["ApiSettings:BaseUrl"];

        public async Task<List<AuditLogDto>>
            GetAllAuditLogsAsync()
        {
            var response =
                await _httpClient.GetFromJsonAsync
                <List<AuditLogDto>>
                (
                    $"{BaseUrl}/AuditLog"
                );

            return response ?? new List<AuditLogDto>();
        }

        public async Task<AuditLogDto?>
            GetAuditLogByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync
            <AuditLogDto>
            (
                $"{BaseUrl}/AuditLog/{id}"
            );
        }
    }
}