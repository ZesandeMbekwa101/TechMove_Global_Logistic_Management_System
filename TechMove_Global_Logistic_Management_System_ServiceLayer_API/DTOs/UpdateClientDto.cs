using System.ComponentModel.DataAnnotations;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class UpdateClientDto
    {
        [Required]
        public int Client_Id { get; set; }

        [Required]
        public string Client_Name { get; set; }

        public string Contact_Person { get; set; }
        public string Phone_Number { get; set; }
        public string Email_Address { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}