using System.Net.Http.Json;
using TechMove_Global_Logistic_Management_System.ViewModels;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            _configuration["ApiSettings:BaseUrl"];

       
        public async Task<bool> LoginAsync(LoginRequestDto dto, HttpContext httpContext)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/Auth/login",
                dto
            );

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (result == null || !result.Success)
                return false;

            // store session (MVC ONLY responsibility)
            httpContext.Session.SetString("AdminUsername", result.Username ?? "");
            httpContext.Session.SetString("AdminId", result.AdminId.ToString());
            httpContext.Session.SetString("Token", result.Token ?? "");

            return true;
        }

        public async Task<bool> LogoutAsync(HttpContext httpContext)
        {
            var token = httpContext.Session.GetString("Token");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Auth/logout");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            httpContext.Session.Clear();

            return response.IsSuccessStatusCode;
        }
    }
}