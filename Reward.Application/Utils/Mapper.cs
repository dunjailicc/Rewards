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
            reward.AgentId = reward.AgentId;
            reward.CustomerId = rewardDto.CusotmerId;
            reward.CampaignId = rewardDto.CampaignId;
            reward.DiscountPercentage = rewardDto.DiscountPercentage;
            reward.CreatedAt = DateTime.UtcNow;

            return reward;
        }

        public static Campaign Map(CampaignDto campaignDto)
        {
            Campaign campaign = new Campaign();

            campaign.Name = campaignDto.Name;
            campaign.ValidFrom = campaignDto.ValidFrom;
            campaign.ValidTo = campaignDto.ValidTo;
            campaign.AdminId = 1; // ToDo - claims

            return campaign;
        }
    }
}
