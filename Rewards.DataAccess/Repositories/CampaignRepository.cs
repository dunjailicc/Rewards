using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface ICampaignRepository
    {
        public Task<Campaign> CreateCampaignAsync(Campaign campaign);

        public Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage);
    }
    public class CampaignRepository : ICampaignRepository
    {   
        private readonly RewardsDbContext _context;

        public CampaignRepository(RewardsDbContext context)
        {
            _context = context;
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
        {   
            var newCampaign = await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();

            return newCampaign.Entity;
        }

        public async Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? adminId, int? pageNumber, int? itemsPerPage)
        {
            throw new NotImplementedException();
        }

    }
}
