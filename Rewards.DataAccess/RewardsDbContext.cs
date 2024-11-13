using Microsoft.EntityFrameworkCore;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess
{
    public class RewardsDbContext : DbContext
    {
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<PurchaseRecord> PurchaseRecords { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Rewards");
        }
    }
}
