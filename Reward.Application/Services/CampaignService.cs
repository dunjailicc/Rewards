using Rewards.Business.DTO;
using Rewards.Business.Helper;
using Rewards.DataAccess.Models;
using Rewards.DataAccess.Repositories;

namespace Rewards.Business.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly CampaignRepository _campaignRepository;
        public CampaignService(CampaignRepository campaignRepository) {
            _campaignRepository = campaignRepository;
        }
        public async Task<Campaign> CreateCampaignAsync(CampaignDto campaignDto)
        {   
            var campaignFromDto = Mapper.Map(campaignDto);
            return await _campaignRepository.CreateCampaignAsync(campaignFromDto);
        }
    }
}
