namespace Rewards.DataAccess.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int DiscountPercentage { get; set; }
        public int CustomerId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime createdAt { get; set; }

        public Campaign Campaign { get; set; }
    }
}
