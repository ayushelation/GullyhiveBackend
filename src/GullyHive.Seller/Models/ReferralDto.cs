namespace GullyHive.Seller.Models
{
    public class ReferralDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int ReferrerUserId { get; set; }
        public string ReferrerRole { get; set; } = string.Empty;
        public string ReferredType { get; set; } = string.Empty;
        public int? ReferredUserId { get; set; }
        public string Source { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // UI fields
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string JoinedDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Earnings { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }

    public class PartnerEarningDto
    {
        public int Id { get; set; }
        public int PartnerUserId { get; set; }
        public int ReferralId { get; set; }
        public int RuleId { get; set; }
        public string EarningType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
