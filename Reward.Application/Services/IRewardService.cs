using Rewards.Application.DTO;
using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.Application.Interfaces
{
    public interface IRewardService
    {
        public Task<Reward> CreateRewardAsync(RewardDto rewardDto);
        public Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage); 
    }
}
