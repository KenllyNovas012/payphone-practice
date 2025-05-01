using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<TransferHistory> TransferHistories { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<User>  Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().ToTable("Wallets");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<TransferHistory>().ToTable("TransfertHistory");

            modelBuilder.Entity<TransferHistory>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transfers)
                .HasForeignKey(t => t.WalletId);
        }
    }
}
