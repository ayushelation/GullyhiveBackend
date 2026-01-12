namespace GullyHive.Admin.Models
{
    public class CityDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = "UK";
        public CityTierEnum Tier { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
