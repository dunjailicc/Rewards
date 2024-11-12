using Rewards.Application.DTO;
using Rewards.Business.DTO;
using Rewards.DataAccess.Models;

namespace Rewards.Business.Helper
{
    public static class Mapper
    {
        public static Reward Map(RewardDto rewardDto)
        {
            Reward reward = new Reward();

            reward.ValidFrom = rewardDto.ValidFrom;
            reward.ValidTo = rewardDto.ValidTo;
            reward.AgentId = 1; // ToDo - claims
            reward.CustomerId = rewardDto.CusotmerId;

            return reward;
        }

        public static Campaign Map(CampaignDto campaignDto)
        {
            Campaign campaign = new Campaign();

            campaign.Name = campaignDto.Name;
            campaign.ValidFrom = campaignDto.ValidFrom;
            campaign.ValidTo = campaignDto.ValidTo;
            campaign.adminId = 1; // ToDo - claims

            return campaign;
        }
    }
}
