using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models
{
    [Table("tblContracts")]
    public class ContractModel
    {
        [Key]
        [Column("contract_Id")]
        public int Contract_Id { get; set; }

        [Required]
        [Column("client_Id")]
        public int Client_Id { get; set; }

        [Required]
        [Column("admin_Id")]
        public int Admin_Id { get; set; }

        [Column("support_Doc_Id")]
        public int? Support_Doc_Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("start_Date")]
        public DateTime Start_Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("end_Date")]
        public DateTime End_Date { get; set; }

        [Required]
        [StringLength(50)]
        [Column("contract_Status")]
        public string Status { get; set; } = "Draft";

        [Required]
        [StringLength(100)]
        [Column("service_Level")]
        public string Service_Level { get; set; }

        [ValidateNever]
        [ForeignKey("Client_Id")]
        public ClientModel Client { get; set; }

        [ValidateNever]
        [ForeignKey("Admin_Id")]
        public AdminModel Admin { get; set; }

        [ValidateNever]
        [ForeignKey("Support_Doc_Id")]
        public SupportDocumentModel SupportDocument { get; set; }

        public ICollection<ServiceRequestModel> ServiceRequests { get; set; } = new List<ServiceRequestModel>();
    }
}