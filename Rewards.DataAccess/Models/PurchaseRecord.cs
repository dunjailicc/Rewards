namespace Rewards.DataAccess.Models
{
    public class PurchaseRecord
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CampaignId { get; set; }

    }
}
