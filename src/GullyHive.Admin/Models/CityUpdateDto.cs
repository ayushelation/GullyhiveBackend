namespace GullyHive.Admin.Models
{
    public class CityUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = "UK";
        public CityTierEnum Tier { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
        public bool IsActive { get; set; }
    }

    public enum CityTierEnum
    {
        X,
        Y,
        Z
    }

}
