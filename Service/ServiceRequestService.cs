using System.Net.Http.Json;
using TechMove_Global_Logistic_Management_System.Models.DTOs;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class ServiceRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ServiceRequestService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            _configuration["ApiSettings:BaseUrl"];

        public async Task<List<ServiceRequestResponseDto>>
            GetAllServiceRequestsAsync()
        {
            var response =
                await _httpClient.GetFromJsonAsync
                <List<ServiceRequestResponseDto>>
                (
                    $"{BaseUrl}/ServiceRequest"
                );

            return response ?? new List<ServiceRequestResponseDto>();
        }

        public async Task<bool>
            AddServiceRequestAsync(CreateServiceRequestDto dto)
        {
            var response =
                await _httpClient.PostAsJsonAsync
                (
                    $"{BaseUrl}/ServiceRequest",
                    dto
                );

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateServiceRequestAsync(UpdateServiceRequestDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"{BaseUrl}/ServiceRequest/UpdateServiceRequest",
                dto
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"{BaseUrl}/ServiceRequest/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}