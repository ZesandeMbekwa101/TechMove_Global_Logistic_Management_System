public class CreateContractDto
{
    public int Client_Id { get; set; }
    public int Admin_Id { get; set; }
    public DateTime Start_Date { get; set; }
    public DateTime End_Date { get; set; }
    public string Status { get; set; }
    public string Service_Level { get; set; }

    public IFormFile? pdfFile { get; set; }
}