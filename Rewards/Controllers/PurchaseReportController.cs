using Microsoft.AspNetCore.Mvc;
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
    }
}
