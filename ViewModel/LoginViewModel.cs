using System.ComponentModel.DataAnnotations;

namespace TechMove_Global_Logistic_Management_System.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}