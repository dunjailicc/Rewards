using Microsoft.AspNetCore.Http;
using Rewards.Business.DTO;

namespace Rewards.Business.Services
{
    public interface IPurchaseReportService
    {
        public Task StoreFileAndSendMessageToQueueAsync(int campaignId, IFormFile file);
        public Task ProcessBatch(List<PurchaseRecordDto> records, int campaignId);

    }
}
