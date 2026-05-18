using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System.Models
{
    [Table("tblAuditLogs")]
    public class AuditLogModel
    {
        [Key]
        [Column("audit_Log_Id")]
        public int Audit_Log_Id { get; set; }

        [Required]
        [Column("admin_Id")]
        public int Admin_Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("action")]
        public string Action { get; set; }

        [Required]
        [StringLength(100)]
        [Column("module")]
        public string Module { get; set; }

        [Required]
        [StringLength(500)]
        [Column("description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Column("ip_Address")]
        public string IP_Address { get; set; }

        [Required]
        [StringLength(50)]
        [Column("status")]
        public string Status { get; set; } = "Success";

        [Required]
        [Column("created_On")]
        public DateTime Created_On { get; set; } = DateTime.Now;

        [ValidateNever]
        [ForeignKey("Admin_Id")]
        public AdminModel Admin { get; set; }
    }
}