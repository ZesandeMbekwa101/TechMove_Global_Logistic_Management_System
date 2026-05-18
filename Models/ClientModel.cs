using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMove_Global_Logistic_Management_System.Models
{
    [Table("tblClients")]
    public class ClientModel
    {
        [Key]
        [Column("client_Id")]
        public int Client_Id { get; set; }

        [Required]
        [StringLength(150)]
        [Column("client_Name")]
        public string Client_Name { get; set; }

        [Required]
        [Column("admin_Id")]
        public int Admin_Id { get; set; }

        [Required]
        [StringLength(150)]
        [Column("contact_Person")]
        public string Contact_Person { get; set; }

        [Required]
        [Phone]
        [Column("phone_Number")]
        public string Phone_Number { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        [Column("email_Address")]
        public string Email_Address { get; set; }

        [Required]
        [StringLength(150)]
        [Column("region")]
        public string Region { get; set; }

        [Required]
        [StringLength(150)]
        [Column("country")]
        public string Country { get; set; }

        [ValidateNever]
        [ForeignKey("Admin_Id")]
        public AdminModel Admin { get; set; }

        public ICollection<ContractModel> Contracts { get; set; } = new List<ContractModel>();
    }
}