namespace GullyHive.Seller.Models
{
    public class SellerDashboardDto
    {
        public long SellerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public SellerStatsDto Stats { get; set; } = new SellerStatsDto();
        public IEnumerable<LeadDto> RecentLeads { get; set; } = new List<LeadDto>();
        public string? ProfilePictureUrl { get; set; }
    }
    public class SellerStatsDto
    {
        public int TotalLeads { get; set; }
        public int TotalResponses { get; set; }
        public decimal TotalEarnings { get; set; }
    }

    public class LeadDto
    {
        public long Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;   // city

        public string? Description { get; set; }

        public decimal? BudgetMin { get; set; }

        public decimal? BudgetMax { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

    public class SellerDto
    {
        public long Id { get; set; }
        public string email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }

    }
}
