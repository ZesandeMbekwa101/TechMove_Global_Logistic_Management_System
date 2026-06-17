namespace TechMove_Global_Logistic_Management_System.Models.DTOs
{
    public class UpdateClientDto
    {
        public int Client_Id { get; set; }
        public string Client_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Phone_Number { get; set; }
        public string Email_Address { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}