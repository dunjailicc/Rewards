using Rewards.Business.DTO;
using Rewards.DataAccess.Models;

namespace Rewards.Business.Services
{
    public interface ICampaignService
    {
        public Task<Campaign> CreateCampaignAsync(CampaignDto campaignDto); 
    }
}
