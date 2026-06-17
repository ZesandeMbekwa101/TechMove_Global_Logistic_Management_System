using System.ComponentModel.DataAnnotations;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class CreateClientDto
    {
        [Required]
        public string Client_Name { get; set; }

        [Required]
        public string Contact_Person { get; set; }

        public string Phone_Number { get; set; }
        public string Email_Address { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public int Admin_Id { get; set; }
    }
}