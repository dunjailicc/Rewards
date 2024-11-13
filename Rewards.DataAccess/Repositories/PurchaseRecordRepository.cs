using Microsoft.EntityFrameworkCore;
using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface IPurchaseRecordRepository
    {
        public Task<PaginatedResult<PurchaseRecord>> GetAsync(int? customerId, int? campaignId, int? pageNumber, int? itemsPerPage);
        public Task<PurchaseRecord> GetByIdAsync(int id);
        public Task AddAsync(List<PurchaseRecord> purchaseRecords);
    }
    public class PurchaseRecordRepository : IPurchaseRecordRepository
    {   
        private readonly RewardsDbContext _context;
        private readonly IPaginationUtils _paginationUtils;

        public PurchaseRecordRepository(RewardsDbContext context, IPaginationUtils paginationUtils)
        {
            _context = context;
            _paginationUtils = paginationUtils;
        }


        public async Task AddAsync(List<PurchaseRecord> purchaseRecords)
        {
            await _context.PurchaseRecords.AddRangeAsync(purchaseRecords);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<PurchaseRecord>> GetAsync(int? customerId, int? campaignId, int? pageNumber, int? itemsPerPage)
        {
            var query = _context.PurchaseRecords.AsQueryable();

            if (customerId != null)
            {
                query = query.Where(r => r.CustomerId == customerId);
            }
            if (campaignId != null)
            {
                query = query.Where(r => r.CampaignId == campaignId);
            }

            var result = await _paginationUtils.ApplyPagination(query, pageNumber, itemsPerPage);

            return result;
        }

        public async Task<PurchaseRecord> GetByIdAsync(int id)
        {
            return await _context.PurchaseRecords.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
