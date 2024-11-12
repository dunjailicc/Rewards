using Microsoft.AspNetCore.Mvc;
using Rewards.Application.DTO;
using Rewards.Application.Interfaces;
using Rewards.DataAccess;

namespace Rewards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardController : ControllerBase
    {
        private readonly RewardsDbContext _context;
        private readonly IRewardService _rewardService;
        private readonly ILogger<RewardController> _logger;

        public RewardController(ILogger<RewardController> logger, RewardsDbContext context, IRewardService rewardService)
        {
            _logger = logger;
            _context = context;
            _rewardService = rewardService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RewardDto reward) { 
            await _rewardService.CreateRewardAsync(reward);
            return Created(); // TODO
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] DateTime? date, [FromQuery] int? agentId, 
            [FromQuery] int? pageNumber, [FromQuery] int? itemsPerPage)
        {
            var rewards = await _rewardService.GetRewardsAsync(date, agentId, pageNumber, itemsPerPage);
            return Ok(rewards);
        }
    }
}
