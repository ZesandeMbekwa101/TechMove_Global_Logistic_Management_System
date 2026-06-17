using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models
{
    [Table("tblServiceRequests")]
    public class ServiceRequestModel
    {
        [Key]
        [Column("service_Request_Id")]
        public int Service_Request_Id { get; set; }

        [Required]
        [Column("contract_Id")]
        public int Contract_Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("service_Description")]
        public string Service_Description { get; set; }

        [Required]
        [Column("usd_Amount", TypeName = "decimal(18,2)")]

        public decimal USD_Amount { get; set; }

        [Required]
        [StringLength(10)]
        [Column("currency_Code")]
        public string Currency_Code { get; set; } = "USD";

        [Required]
        [Column("exchange_Rate", TypeName = "decimal(18,4)")]
        public decimal Exchange_Rate { get; set; }

        [Required]
        [Column("zar_Amount", TypeName = "decimal(18,2)")]
        public decimal ZAR_Amount { get; set; }

        [Required]
        [StringLength(50)]
        [Column("status")]
        public string Status { get; set; } = "Pending";

        [Required]
        [Column("created_On")]
        public DateTime Created_On { get; set; } = DateTime.Now;

        [ValidateNever]
        [ForeignKey("Contract_Id")]
        public ContractModel Contract { get; set; }
    }
}