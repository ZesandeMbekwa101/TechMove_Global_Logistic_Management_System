using System.ComponentModel.DataAnnotations;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}