using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System.Models
{
    [Table("tblAdmin")]
    public class AdminModel
    {
        [Key]
        [Column("admin_Id")]
        public int Admin_Id { get; set; }

        [Required]
        [StringLength(150)]
        [Column("first_Name")]
        public string First_Name { get; set; }

        [Required]
        [StringLength(150)]
        [Column("last_Name")]
        public string Last_Name { get; set; }

        [Required]
        [StringLength(150)]
        [Column("user_Name")]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [Column("password_hash")]
        public string Password_Hash { get; set; }

        [Required]
        [Column("created_On")]
        public DateTime Created_On { get; set; } = DateTime.Now;

        public ICollection<ClientModel> Clients { get; set; } = new List<ClientModel>();
        public ICollection<ContractModel> Contracts { get; set; } = new List<ContractModel>();
        public ICollection<AuditLogModel> AuditLogs { get; set; } = new List<AuditLogModel>();
    }
}