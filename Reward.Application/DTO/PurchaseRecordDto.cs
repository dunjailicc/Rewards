namespace Rewards.Business.DTO
{
    public class PurchaseRecordDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CampaignId { get; set; }
    }

    public class CreatePurchaseRecordDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CampaignId { get; set; }
    }
}
