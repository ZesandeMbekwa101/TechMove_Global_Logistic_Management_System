namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services
{
    public class FileValidationService
    {
        public void ValidatePdfFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name is required.");

            var extension = Path.GetExtension(fileName).ToLower();

            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");
        }
    }
}