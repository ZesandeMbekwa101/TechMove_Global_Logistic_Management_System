namespace TechMove_Global_Logistic_Management_System.Models.DTOs
{
    public class CreateServiceRequestDto
    {
        public int Contract_Id { get; set; }

        public string Service_Description { get; set; }

        public decimal ZAR_Amount { get; set; }

        public string Target_Currency { get; set; }

        public string Status { get; set; }
    }
}