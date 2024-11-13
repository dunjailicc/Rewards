namespace Rewards.Business.DTO.Filters
{
    public class PurchaseReportFilter
    {
        public int? CampaignId { get; set; }
        public int? CustomerId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
