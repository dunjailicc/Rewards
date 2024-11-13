using Microsoft.AspNetCore.Http;
using Rewards.Application.Pagination;
using Rewards.Business.DTO;
using Rewards.Business.DTO.Filters;

namespace Rewards.Business.Services
{
    public interface IPurchaseReportService
    {
        public Task<PaginatedResult<PurchaseRecordDto>> GetAsync(PurchaseReportFilter filter);
        public Task<PurchaseRecordDto> GetByIdAsync(int id);
        public Task StoreFileAndSendMessageToQueueAsync(int campaignId, IFormFile file);
        public Task ProcessBatch(List<CreatePurchaseRecordDto> records, int campaignId);

    }
}
