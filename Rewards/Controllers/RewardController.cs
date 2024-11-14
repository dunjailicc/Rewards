using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rewards.Application.DTO;
using Rewards.Application.Interfaces;

namespace Rewards.Controllers
{
    //[Authorize(Roles = "agent")]
    [ApiController]
    [Route("api/[controller]")]
    public class RewardController : ControllerBase
    {
        private readonly IRewardService _rewardService;
        private readonly ILogger<RewardController> _logger;

        public RewardController(ILogger<RewardController> logger, IRewardService rewardService)
        {
            _logger = logger;
            _rewardService = rewardService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RewardDto rewardDto) {
            if (rewardDto == null)
            {
                return BadRequest();
            }
            var createdReward = await _rewardService.CreateRewardAsync(rewardDto);
            return Ok(createdReward);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] DateTime? date, [FromQuery] int? agentId,
            [FromQuery] int? pageNumber, [FromQuery] int? itemsPerPage)
        {
            var rewards = await _rewardService.GetRewardsAsync(date, agentId, pageNumber, itemsPerPage);
            return Ok(rewards);
        }

        [HttpPatch("{rewardId}")]
        public async Task<IActionResult> PatchAsync(int rewardId, [FromQuery] RewardDto rewardDto)
        {
            var updatedReward = await _rewardService.UpdateRewardAsync(rewardId, rewardDto);
            // TODO - not updated
            if (updatedReward == null)
            {
                return NotFound();
            }
            return Ok(updatedReward);
        }

        [HttpDelete("{rewardId}")]
        public async Task<IActionResult> DeleteAsync(int rewardId)
        {
            await _rewardService.DeleteRewardAsync(rewardId);
            return NoContent();
        }

    }        
}
