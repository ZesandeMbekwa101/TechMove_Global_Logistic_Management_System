namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models
{
    public class FilterModel
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}