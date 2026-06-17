namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class CurrencyConversionResponseDto
    {
        public string Base { get; set; }
        public string Target { get; set; }

        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }

        public decimal Rate { get; set; }
    }
}