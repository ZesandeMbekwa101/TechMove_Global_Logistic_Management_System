namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class UpdateServiceRequestDto
    {
        // REQUIRED for MVC form binding consistency
        public int Service_Request_Id { get; set; }

        public int Contract_Id { get; set; }

        public string Service_Description { get; set; }

        public string Status { get; set; }

        // FX INPUT
        public decimal ZAR_Amount { get; set; }

        // IMPORTANT: keep consistency with Create
        public string Target_Currency { get; set; } = "USD";

      
    }
}