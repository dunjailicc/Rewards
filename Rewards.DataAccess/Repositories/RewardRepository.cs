using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface IRewardRepository
    {
        public Task<Reward> CreateRewardAsync(Reward reward);

        public Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage);

        public Task<Reward> UpdateRewardAsync(int rewardId, Reward reward);

        public Task DeleteRewardAsync(int rewardId);
    }

    public class RewardRepository : IRewardRepository
    {
        private readonly RewardsDbContext _dbContext;
        private readonly IPaginationUtils _paginationUtils;

        public RewardRepository(RewardsDbContext dbContext, IPaginationUtils paginationUtils)
        {
            _dbContext = dbContext;
            _paginationUtils = paginationUtils;
        }

        public async Task<Reward> CreateRewardAsync(Reward reward)
        {
            var newReward = await _dbContext.Rewards.AddAsync(reward);
            await _dbContext.SaveChangesAsync(); // TODO

            return newReward.Entity;
        }

        public async Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage)
        {
            var query = _dbContext.Rewards.AsQueryable();

            if (agentId != null && date == null)
            {   
                query = query.Where(r => r.AgentId == agentId);

            }
            else if(date != null && agentId == null)
            {
                query = query.Where(r => r.ValidFrom == date);
            }
            else if (date != null && agentId != null)
            {
                query = query.Where(r => r.AgentId == agentId && r.ValidFrom == date);
            }

            var result = await _paginationUtils.ApplyPagination(query, pageNumber, itemsPerPage);

            return result;
        }

        public async Task<Reward> UpdateRewardAsync(int rewardId, Reward reward)
        {   
            var existingReward = await _dbContext.Rewards.FindAsync(rewardId);

            if(existingReward == null) { 
                // TODO throw Exception
            }

            existingReward.CustomerId = reward.CustomerId;
            existingReward.DiscountPercentage = reward.DiscountPercentage;
            // TODO validate
            existingReward.ValidFrom = reward.ValidFrom;
            existingReward.ValidTo = reward.ValidTo;

            await _dbContext.SaveChangesAsync();

            return existingReward;
        }

        public async Task DeleteRewardAsync(int rewardId)
        {
            var rewardById = await _dbContext.Rewards.FindAsync(rewardId);

            if(rewardById == null) {
                // TODO throw Exception();
            }

            _dbContext.Rewards.Remove(rewardById);
            await _dbContext.SaveChangesAsync();
        }

    }
}
