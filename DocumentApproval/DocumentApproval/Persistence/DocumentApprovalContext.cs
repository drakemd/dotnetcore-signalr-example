using DocumentApproval.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentApproval.Persistence
{
    public class DocumentApprovalContext: DbContext
    {
        public DocumentApprovalContext() { }

        public DocumentApprovalContext(DbContextOptions<DocumentApprovalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.DocumentApproval> DocumentApprovals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=tcp:pocbuma.database.windows.net,1433;Initial Catalog=elsasample;Persist Security Info=False;User ID=bumaadmin;Password=Buma,12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.DocumentApproval>(entity =>
            {
                entity.ToTable("DocumentApproval");
            });
        }
    }
}
