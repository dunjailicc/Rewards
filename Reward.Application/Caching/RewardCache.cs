using Microsoft.Extensions.Caching.Memory;

namespace Rewards.Business.Caching
{
    public interface IRewardCache
    {
        public int GetAgentRewardCount(int agentId);
        public void IncrementAgentRewardCount(int agentId);
    }
    public class RewardCache : IRewardCache
    {
        IMemoryCache _memoryCache;
        public RewardCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public int GetAgentRewardCount(int agentId)
        {
            var cacheKey = $"AgentRewardCount_{agentId}_{DateTime.UtcNow.ToString("d")}";

            if (!_memoryCache.TryGetValue(cacheKey, out int currentCount)) { currentCount = 0; }

            return currentCount;
        }

        public void IncrementAgentRewardCount(int agentId)
        {
            var cacheKey = $"AgentRewardCount_{agentId}_{DateTime.UtcNow.ToString("d")}";
            var currentCount = GetAgentRewardCount(agentId);
            _memoryCache.Set(cacheKey, currentCount + 1, TimeSpan.FromHours(24));
        }
    }

}
