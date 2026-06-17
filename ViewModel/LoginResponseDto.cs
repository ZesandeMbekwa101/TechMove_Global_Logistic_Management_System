namespace TechMove_Global_Logistic_Management_System.ViewModels
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}