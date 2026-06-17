using System.Text.Json;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services
{
    public class FastForexService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FastForexService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CurrencyConversionResponseDto?> ConvertZarToCurrencyAsync(
            decimal zarAmount,
            string targetCurrency)
        {
            var apiKey = _configuration["FastForex:ApiKey"];

            var url =
                $"https://api.fastforex.io/fetch-one?from=ZAR&to={targetCurrency}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-API-Key", apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var result = doc.RootElement.GetProperty("result");

            // ✅ THIS IS THE REAL RATE (NOT converted value)
            decimal rate = result.GetProperty(targetCurrency).GetDecimal();

            // ✅ CORRECT CALCULATION
            decimal convertedAmount = zarAmount * rate;

            return new CurrencyConversionResponseDto
            {
                Base = "ZAR",
                Target = targetCurrency,
                Amount = zarAmount,
                ConvertedAmount = Math.Round(convertedAmount, 2),
                Rate = Math.Round(rate, 6)
            };
        }
    }
}