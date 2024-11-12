using Rewards.Application.DTO;
using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Helper;
using Rewards.DataAccess.Models;
using Rewards.DataAccess.Repositories;

namespace Rewards.Business.Services
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _rewardRepository;

        public RewardService(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<Reward> CreateRewardAsync(RewardDto rewardDto)
        {   
            var rewardFromDto = Mapper.Map(rewardDto);
            var createdReward = await _rewardRepository.CreateRewardAsync(rewardFromDto);

            return createdReward;
        }

        public async Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage)
        {
            var rewards = await _rewardRepository.GetRewardsAsync(date, agentId, pageNumber, itemsPerPage);
            return rewards;
        }

        public async Task<Reward> UpdateRewardAsync(int id, RewardDto rewardDto)
        {   
            var rewardFromDto = Mapper.Map(rewardDto);
            var updatedReward = await _rewardRepository.UpdateRewardAsync(id, rewardFromDto);

            return updatedReward;
        }

        public async Task DeleteRewardAsync(int rewardId)
        {
            await _rewardRepository.DeleteRewardAsync(rewardId);
        }

    }
}
