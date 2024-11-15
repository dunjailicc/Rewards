﻿namespace Rewards.DataAccess.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<PurchaseRecord> PurchaseRecords { get; set; }
    }
}
