using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface ICampaignRepository
    {
        public Task<Campaign> CreateCampaignAsync(Campaign campaign);

        public Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage);
        public Task<Campaign> GetCampaignByIdAsync(int campaignId);
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

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {
            var campaign = await _dbContext.Campaigns.FindAsync(campaignId);
            if(campaign == null) 
            { 
                // throw Exception
                                       
            }

            return campaign;
        }

        public async Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? adminId, int? pageNumber, int? itemsPerPage)
        {
            var query = _dbContext.Campaigns.AsQueryable();

            if (adminId != null && date == null)
            {
                query = query.Where(c => c.AdminId == adminId);

            }
            else if (date != null && adminId == null)
            {
                query = query.Where(r => r.ValidFrom == date);
            }
            else if (date != null && adminId != null)
            {
                query = query.Where(r => r.AdminId == adminId && r.ValidFrom == date);
            }

            var result = await _paginationUtils.ApplyPagination(query, pageNumber, itemsPerPage);

            return result;
        }

    }
}
