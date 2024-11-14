using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface ICampaignRepository
    {
        public Task<Campaign> CreateCampaignAsync(Campaign campaign);
        public Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage);
        public Task<Campaign> GetCampaignByIdAsync(int campaignId);
        public Task DeleteCampaignAsync(int campaignId);
    }
    public class CampaignRepository : ICampaignRepository
    {   
        private readonly RewardsDbContext _dbContext;
        private readonly IPaginationUtils _paginationUtils;

        public CampaignRepository(RewardsDbContext dBcontext, IPaginationUtils paginationUtils)
        {
            _dbContext = dBcontext;
            _paginationUtils = paginationUtils;
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
        {   
            var newCampaign = await _dbContext.Campaigns.AddAsync(campaign);
            await _dbContext.SaveChangesAsync();

            return newCampaign.Entity;
        }

        public async Task DeleteCampaignAsync(int campaignId)
        {
            var campaignById = await _dbContext.Campaigns.FindAsync(campaignId);

            _dbContext.Campaigns.Remove(campaignById);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId) => await _dbContext.Campaigns.FindAsync(campaignId);

        public async Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? adminId, int? pageNumber, int? itemsPerPage)
        {
            var query = _dbContext.Campaigns.AsQueryable();

            if (adminId is not null)
            {
                query = query.Where(c => c.AdminId == adminId);

            }
            if (date is not null)
            {
                query = query.Where(r => r.ValidFrom == date);
            }

            var result = await _paginationUtils.ApplyPagination(query, pageNumber, itemsPerPage);

            return result;
        }

    }
}
