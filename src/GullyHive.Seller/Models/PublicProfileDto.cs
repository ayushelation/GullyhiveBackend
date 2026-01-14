//namespace GullyHive.Seller.Models
//{

//    public class PublicProfileDto
//    {
//        public long SellerId { get; set; }
//        public string LegalName { get; set; } = string.Empty;
//        public string DisplayName { get; set; } = string.Empty;
//        public string Email { get; set; } = string.Empty;
//        public string Phone { get; set; } = string.Empty;
//        public string ProviderType { get; set; } = string.Empty;
//        public string Status { get; set; } = string.Empty;
//        public string BaseCity { get; set; } = string.Empty;
//        public decimal AvgRating { get; set; }
//        public int RatingCount { get; set; }

//        // ✅ Address (Primary)
//        public long? AddressId { get; set; }
//        public string? AddressLabel { get; set; }
//        public string? AddressLine1 { get; set; }
//        public string? AddressLine2 { get; set; }
//        public string? Locality { get; set; }
//        public string? AddressCity { get; set; }
//        public string? State { get; set; }
//        public string? Pincode { get; set; }
//        public string? Description { get; set; }
//    }

//    public class ReviewDto
//    {
//        public string ReviewerName { get; set; } = string.Empty;
//        public int Rating { get; set; }
//        public string Comment { get; set; } = string.Empty;
//        public DateTime CreatedAt { get; set; }
//    }
//    public class UpdateProfileDto
//    {
//        public string DisplayName { get; set; } = string.Empty;
//        public string Email { get; set; } = string.Empty;
//        public string Phone { get; set; } = string.Empty;
//        public string Description { get; set; } = string.Empty;

//        public string AddressLine1 { get; set; } = string.Empty;
//        public string City { get; set; } = string.Empty;
//        public string State { get; set; } = string.Empty;
//        public string Pincode { get; set; } = string.Empty;
//    }



//}
namespace GullyHive.Seller.Models
{
    public class PublicProfileDto
    {
        // Basic Info
        public long SellerId { get; set; }
        public string LegalName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ProviderType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string BaseCity { get; set; } = string.Empty;
        public string? State { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
        public int TotalJobsCompleted { get; set; }
        public int TotalDisputes { get; set; }
        public decimal DisputeRate { get; set; }

        // Primary Address
        public long? AddressId { get; set; }
        public string? AddressLabel { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Locality { get; set; }
        public string? AddressCity { get; set; }
        public string? Pincode { get; set; }
        public string? Description { get; set; }

        // Dynamic Data
        public List<string> Services { get; set; } = new();          // service names
        public List<string> PortfolioImages { get; set; } = new();   // URLs
        public List<ReviewDto> Reviews { get; set; } = new();        // reviews
    }

    public class ReviewDto
    {
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    public class UpdateProfileDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
    }
}

