namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class AuditLogDto
    {
        public int Audit_Log_Id { get; set; }
        public int Admin_Id { get; set; }
        public string Admin_Name { get; set; } // optional but useful
        public string Action { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public string IP_Address { get; set; }
        public string Status { get; set; }
        public DateTime Created_On { get; set; }
    }
}