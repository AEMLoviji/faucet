using Microsoft.EntityFrameworkCore;
using System;

namespace Faucet.API.Data
{
    public class FaucetDbContext : DbContext
    {
        public DbSet<Balance> Balance { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Transaction> Transaction { get; set; }        

        public FaucetDbContext(DbContextOptions<FaucetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>()
                .HasData(new Balance { Id = 1, BitcoinsAmount = 0, UpdatedAt = DateTime.UtcNow });
        }
    }
}
