using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Models._Users;

namespace Services.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// DbSet cho các bảng News domain (tạo sau)
        /// </summary>
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentBranch> DocumentBranchs { get; set; }
        public DbSet<BorrowRequest> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
