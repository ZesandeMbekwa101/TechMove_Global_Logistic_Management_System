namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? AdminId { get; set; }
        public string? Username { get; set; }
    }
}