namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class CreateAuditLogDto
    {
        public int Admin_Id { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Success";
    }
}