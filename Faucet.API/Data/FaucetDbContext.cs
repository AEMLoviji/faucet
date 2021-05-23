using Microsoft.EntityFrameworkCore;
using System;

namespace Faucet.API.Data
{
    public class FaucetDbContext : DbContext
    {
        public DbSet<Balance> Balance { get; set; }

        public FaucetDbContext(DbContextOptions<FaucetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>()
                .HasData(new Balance { Id = 1, BitcoinsCount = 0, UpdatedAt = DateTime.UtcNow });
        }
    }
}
