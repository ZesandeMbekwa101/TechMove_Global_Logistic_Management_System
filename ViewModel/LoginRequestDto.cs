using System.ComponentModel.DataAnnotations;

namespace TechMove_Global_Logistic_Management_System.ViewModels
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}