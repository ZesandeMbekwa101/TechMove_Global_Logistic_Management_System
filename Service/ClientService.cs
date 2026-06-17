using System.Net.Http.Json;
using TechMove_Global_Logistic_Management_System.Models.DTOs;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ClientService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            _configuration["ApiSettings:BaseUrl"];

        public async Task<List<ClientResponseDto>> GetAllClientsAsync(
            string? search = null,
            string? region = null)
        {
            var endpoint =
                $"{BaseUrl}/Client/GetAllClients?Search={search}&region={region}";

            var clients =
                await _httpClient.GetFromJsonAsync<List<ClientResponseDto>>(endpoint);

            return clients ?? new List<ClientResponseDto>();
        }

        public async Task<bool> AddClientAsync(CreateClientDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/Client/AddClient",
                dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateClientAsync(UpdateClientDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"{BaseUrl}/Client/UpdateClient",
                dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"{BaseUrl}/Client/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}