﻿namespace Rewards.Application.DTO
{
    public class RewardDto
    {
        public int CusotmerId { get; set; }
        public int DiscountPercentage { get; set; }
        public int CampaignId { get; set; }
        public int AgentId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
