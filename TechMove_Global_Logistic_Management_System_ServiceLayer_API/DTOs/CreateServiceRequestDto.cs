namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class CreateServiceRequestDto
    {
        public int Contract_Id { get; set; }

        public string Service_Description { get; set; }

        // USER INPUT (ALWAYS ZAR)
        public decimal ZAR_Amount { get; set; }

        // TARGET CURRENCY (USD, EUR, GBP, etc.)
        public string Target_Currency { get; set; } = "USD";

        public string Status { get; set; } = "Pending";
    }
}