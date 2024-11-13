using Rewards.Application.Pagination;
using Rewards.Business.DTO;
using Rewards.DataAccess.Models;

namespace Rewards.Business.Services
{
    public interface ICampaignService
    {
        public Task<Campaign> CreateCampaignAsync(CampaignDto campaignDto);
        public Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? adminId, int? pageNumber, int? itemsPerPage);
        public Task<Campaign> GetCampaignByIdAsync(int campaignId);
        public Task<Campaign> UpdateCampaignAsync(int campaignId, CampaignDto campaignDto);
        public Task DeleteCampaignAsync(int campaignId);
    }
}
