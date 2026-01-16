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
        public long? ProviderId { get; set; }
        public long? SellerId { get; set; }

        // public long ProviderId { get; set; }
        //public long SellerId { get; set; }

        public string? LegalName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? ProviderType { get; set; }
        public string? Status { get; set; }
        public string? BaseCity { get; set; }
        public string? State { get; set; }
        public string? Description { get; set; }

        public decimal? AvgRating { get; set; }
        public long? RatingCount { get; set; }
        public int? TotalJobsCompleted { get; set; }
        public int? TotalDisputes { get; set; }
        public decimal? DisputeRate { get; set; }

        // 🔥 THESE MUST BE NULLABLE
        public long? AddressId { get; set; }
        public string? AddressLabel { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Locality { get; set; }
        public string? AddressCity { get; set; }
        public string? Pincode { get; set; }

        //public string Services { get; set; } = "[]";
        //public string PortfolioImages { get; set; } = "[]";
        //public string Reviews { get; set; } = "[]";
        public List<string> Services { get; set; } = new();
            public List<string> PortfolioImages { get; set; } = new();
           public List<ReviewDto> Reviews { get; set; } = new();
        public DateTime? CreatedAt { get; set; }
        public int? ReviewsCount { get; set; }
        public string? ProfilePictureUrl { get; set; }

    }


    public class ReviewDto
    {
        public string ReviewerName { get; set; } = string.Empty;
        public decimal Rating { get; set; }
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
        public IFormFile? ProfilePicture { get; set; }
    }
}


