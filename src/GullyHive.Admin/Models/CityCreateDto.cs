namespace GullyHive.Admin.Models
{
    public class CityCreateDto
    {
        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = "UK";
        public CityTierEnum Tier { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
    }
  

}
