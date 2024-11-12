using Rewards.Application.DTO;
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
            reward.AgentId = 1; // ToDo 
            reward.CustomerId = rewardDto.CusotmerId;

            return reward;
            //throw new NotImplementedException();
        }
    }
}
