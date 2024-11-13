using Microsoft.AspNetCore.Mvc;
using Rewards.Business.DTO;
using Rewards.Business.Services;
using Rewards.Controllers;

namespace Rewards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        public readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(ILogger<CampaignController> logger, ICampaignService campaignService) { 
            _logger = logger;
            _campaignService = campaignService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CampaignDto campaignDto)
        {
            if (campaignDto == null)
            {
                return BadRequest(); 
            }

            var createdCampaign = await _campaignService.CreateCampaignAsync(campaignDto);
            return Ok(createdCampaign);
        }



    }
}
