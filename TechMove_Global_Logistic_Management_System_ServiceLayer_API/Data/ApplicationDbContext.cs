using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<ContractModel> Contracts { get; set; }
        public DbSet<SupportDocumentModel> SupportDocuments { get; set; }
        public DbSet<ServiceRequestModel> ServiceRequests { get; set; }
        public DbSet<AuditLogModel> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminModel>().ToTable("tblAdmin");
            modelBuilder.Entity<ClientModel>().ToTable("tblClients");
            modelBuilder.Entity<ContractModel>().ToTable("tblContracts");
            modelBuilder.Entity<ServiceRequestModel>().ToTable("tblServiceRequests");
            modelBuilder.Entity<SupportDocumentModel>().ToTable("tblSupportDocuments");
            modelBuilder.Entity<AuditLogModel>().ToTable("tblAuditLogs");

            // CLIENT -> ADMIN
            modelBuilder.Entity<ClientModel>()
                .HasOne(c => c.Admin)
                .WithMany(a => a.Clients)
                .HasForeignKey(c => c.Admin_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTRACT -> ADMIN
            modelBuilder.Entity<ContractModel>()
                .HasOne(c => c.Admin)
                .WithMany(a => a.Contracts)
                .HasForeignKey(c => c.Admin_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTRACT -> CLIENT
            modelBuilder.Entity<ContractModel>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Contracts)
                .HasForeignKey(c => c.Client_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTRACT -> SUPPORT DOCUMENT
            modelBuilder.Entity<ContractModel>()
                .HasOne(c => c.SupportDocument)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.Support_Doc_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // SERVICE REQUEST -> CONTRACT
            modelBuilder.Entity<ServiceRequestModel>()
                .HasOne(s => s.Contract)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(s => s.Contract_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // AUDIT LOG -> ADMIN
            modelBuilder.Entity<AuditLogModel>()
                .HasOne(a => a.Admin)
                .WithMany(a => a.AuditLogs)
                .HasForeignKey(a => a.Admin_Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
