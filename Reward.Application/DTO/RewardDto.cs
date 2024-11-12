namespace Rewards.Application.DTO
{
    public class RewardDto
    {
        public int CusotmerId { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
