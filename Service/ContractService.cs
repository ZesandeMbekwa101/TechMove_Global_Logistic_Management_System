using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMove_Global_Logistic_Management_System.Models.DTOs;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class ContractService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ContractService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            _configuration["ApiSettings:BaseUrl"];

        public async Task<List<ContractResponseDto>> GetAllContractsAsync()
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<ContractResponseDto>>
                (
                    $"{BaseUrl}/Contract/GetAllContracts"
                );

            return response ?? new List<ContractResponseDto>();
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"{BaseUrl}/Contract/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddContractAsync(CreateContractDto dto)
        {
            var form = new MultipartFormDataContent();

            form.Add(
                new StringContent(dto.Client_Id.ToString()),
                "Client_Id");

            form.Add(
                new StringContent(dto.Admin_Id.ToString()),
                "Admin_Id");

            form.Add(
                new StringContent(dto.Start_Date.ToString("yyyy-MM-dd")),
                "Start_Date");

            form.Add(
                new StringContent(dto.End_Date.ToString("yyyy-MM-dd")),
                "End_Date");

            form.Add(
                new StringContent(dto.Status ?? ""),
                "Status");

            form.Add(
                new StringContent(dto.Service_Level ?? ""),
                "Service_Level");

            if (dto.pdfFile != null)
            {
                var stream = dto.pdfFile.OpenReadStream();

                form.Add(
                    new StreamContent(stream),
                    "pdfFile",
                    dto.pdfFile.FileName);
            }

            var response =
                await _httpClient.PostAsync(
                    $"{BaseUrl}/Contract/AddContract",
                    form);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateContractAsync(UpdateContractDto dto)
        {
            var form = new MultipartFormDataContent();

            form.Add(
                new StringContent(dto.Contract_Id.ToString()),
                "Contract_Id");

            form.Add(
                new StringContent(dto.Client_Id.ToString()),
                "Client_Id");

            form.Add(
                new StringContent(dto.Admin_Id.ToString()),
                "Admin_Id");

            form.Add(
                new StringContent(dto.Start_Date.ToString("yyyy-MM-dd")),
                "Start_Date");

            form.Add(
                new StringContent(dto.End_Date.ToString("yyyy-MM-dd")),
                "End_Date");

            form.Add(
                new StringContent(dto.Status ?? ""),
                "Status");

            form.Add(
                new StringContent(dto.Service_Level ?? ""),
                "Service_Level");

            if (dto.pdfFile != null)
            {
                var stream = dto.pdfFile.OpenReadStream();

                form.Add(
                    new StreamContent(stream),
                    "pdfFile",
                    dto.pdfFile.FileName);
            }

            var request =
                new HttpRequestMessage(
                    HttpMethod.Put,
                    $"{BaseUrl}/Contract/UpdateContract")
                {
                    Content = form
                };

            var response =
                await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}