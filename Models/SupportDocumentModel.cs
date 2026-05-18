using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System.Models
{
    [Table("tblSupportDocuments")]
    public class SupportDocumentModel
    {
        [Key]
        [Column("support_Doc_Id")]
        public int Support_Doc_Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("file_Name")]
        public string File_Name { get; set; }

        [Required]
        [StringLength(500)]
        [Column("file_Path")]
        public string File_Path { get; set; }

        [StringLength(50)]
        [Column("file_Type")]
        public string File_Type { get; set; } = ".pdf";

        [Column("uploaded_On")]
        public DateTime Uploaded_On { get; set; } = DateTime.Now;

        public ICollection<ContractModel> Contracts { get; set; } = new List<ContractModel>();
    }
}