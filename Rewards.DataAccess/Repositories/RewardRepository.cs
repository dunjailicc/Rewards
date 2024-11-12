using Rewards.Application.Pagination;
using Rewards.DataAccess.Models;

namespace Rewards.DataAccess.Repositories
{
    public interface IRewardRepository
    {
        public Task<Reward> CreateRewardAsync(Reward reward);

        public Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage);
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
           // throw new NotImplementedException();
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
    }
}
