using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface IPurchaseRecordRepository
    {
        public Task AddAsync(List<PurchaseRecord> purchaseRecords);
    }
    public class PurchaseRecordRepository : IPurchaseRecordRepository
    {   
        private readonly RewardsDbContext _context;

        public PurchaseRecordRepository(RewardsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(List<PurchaseRecord> purchaseRecords)
        {
            await _context.PurchaseRecords.AddRangeAsync(purchaseRecords);
            await _context.SaveChangesAsync();

            var tst = _context.PurchaseRecords.ToList();

            Console.Write("1");
        }
    }
}
