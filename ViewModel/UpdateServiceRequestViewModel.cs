namespace TechMove_Global_Logistic_Management_System.ViewModel
{
    public class UpdateServiceRequestViewModel
    {
        public int Service_Request_Id { get; set; }
        public int Contract_Id { get; set; }
        public string Service_Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency_Code { get; set; }
        public decimal Exchange_Rate { get; set; }
        public decimal ZAR_Amount { get; set; }
        public string Status { get; set; }
    }
}