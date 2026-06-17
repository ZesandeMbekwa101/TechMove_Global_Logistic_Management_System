namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs
{
    public class ContractDto
    {
        public int Contract_Id { get; set; }
        public int Client_Id { get; set; }
        public int Admin_Id { get; set; }
        public int? Support_Doc_Id { get; set; }

        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }

        public string Status { get; set; }
        public string Service_Level { get; set; }

        public string? File_Name { get; set; }
        public string? File_Path { get; set; }
    }
}