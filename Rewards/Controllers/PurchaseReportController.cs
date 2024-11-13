using Microsoft.AspNetCore.Mvc;
using Rewards.Business.DTO.Filters;
using Rewards.Business.Services;

namespace Rewards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseReportController : ControllerBase
    {
        private readonly IPurchaseReportService _purchaseReportService;
        public PurchaseReportController(IPurchaseReportService purchaseReportService)
        {
            _purchaseReportService = purchaseReportService;
        }

        [HttpPost("{campaignId}")]
        public async Task<IActionResult> StoreFileAndSendMessageToQueueAsync(int campaignId, IFormFile file)
        {
            await _purchaseReportService.StoreFileAndSendMessageToQueueAsync(campaignId, file);
            return Accepted();
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PurchaseReportFilter filter = null)
        {
            if (filter == null)
                filter = new PurchaseReportFilter { PageNumber = 1, PageSize = 100 };

            return Ok(await _purchaseReportService.GetAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _purchaseReportService.GetByIdAsync(id));
        }
    }
}
