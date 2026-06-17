using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;
using Xunit;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Tests
{
    public class FileValidationServiceTests
    {
        [Fact]
        public void ValidatePdfFile_WithValidPdf_ShouldPass()
        {
            // Arrange
            var service = new FileValidationService();

            // Act
            service.ValidatePdfFile("contract.pdf");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void ValidatePdfFile_WithUppercasePdfExtension_ShouldPass()
        {
            // Arrange
            var service = new FileValidationService();

            // Act
            service.ValidatePdfFile("AGREEMENT.PDF");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void ValidatePdfFile_WithDocxFile_ShouldThrowException()
        {
            // Arrange
            var service = new FileValidationService();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.ValidatePdfFile("document.docx"));
        }

        [Fact]
        public void ValidatePdfFile_WithExeFile_ShouldThrowException()
        {
            // Arrange
            var service = new FileValidationService();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.ValidatePdfFile("virus.exe"));
        }

        [Fact]
        public void ValidatePdfFile_WithEmptyFileName_ShouldThrowException()
        {
            // Arrange
            var service = new FileValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                service.ValidatePdfFile(""));
        }

        [Fact]
        public void ValidatePdfFile_WithNullFileName_ShouldThrowException()
        {
            // Arrange
            var service = new FileValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                service.ValidatePdfFile(null));
        }

        [Fact]
        public void ValidatePdfFile_WithWhitespaceFileName_ShouldThrowException()
        {
            // Arrange
            var service = new FileValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                service.ValidatePdfFile("   "));
        }
    }
}