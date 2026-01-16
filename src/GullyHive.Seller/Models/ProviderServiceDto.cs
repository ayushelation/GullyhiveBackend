//using GullyHive.Admin.Models;
//using GullyHive.Auth.Models;

//namespace GullyHive.Seller.Models
//{
//    public class ProviderServiceDto
//    {
//        public long CategoryId { get; set; }
//        public List<long> SubCategoryIds { get; set; } = new(); // for update
//    }

//    public class ProviderServiceAreaDto
//    {
//        public string Type { get; set; } = "";
//        public long? CityId { get; set; }
//        public double? RadiusKm { get; set; }
//        public List<string> Pincodes { get; set; } = new();
//    }
//    public class CityDto
//    {
//        public long Id { get; set; }
//        public string Name { get; set; } = "";
//        public string State { get; set; } = "";
//    }

//    // Full GET response DTO
//    public class ProviderServicesInitDto
//    {
//        public List<ProviderServiceDto> ProviderServices { get; set; } = new();
//        public ProviderServiceAreaDto ServiceArea { get; set; } = new();
//        public List<CategoryDto> Categories { get; set; } = new();
//        public List<SubCategoryDto> SubCategories { get; set; } = new();
//        public List<CityDto> Cities { get; set; } = new();
//    }

//    // Update payload DTO
//    public class UpdateProviderServicesDto
//    {
//        public List<ProviderServiceDto> Services { get; set; } = new();
//        public ProviderServiceAreaDto ServiceArea { get; set; } = new();
//    }

//}
namespace GullyHive.Seller.Models
{
    // Service per category with multiple subcategories
    public class ProviderServiceDto
    {
        public long CategoryId { get; set; }
        public List<long> SubCategoryIds { get; set; } = new();
    }

    // Service area
    public class ProviderServiceAreaDto
    {
        public string Type { get; set; } = ""; // city | radius | pincode
        public long? CityId { get; set; }
        public double? RadiusKm { get; set; }
        public List<string> Pincodes { get; set; } = new();
    }

    // GET response DTO
    public class ProviderServicesInitDto
    {
        public List<ProviderServiceDto> ProviderServices { get; set; } = new();
        public ProviderServiceAreaDto ServiceArea { get; set; } = new();
        public List<PCategoryDto> Categories { get; set; } = new();
        public List<SubCategoryDto> SubCategories { get; set; } = new();
        public List<CityDto> Cities { get; set; } = new();
    }

    // Update request DTO
    public class UpdateProviderServicesDto
    {
        public List<ProviderServiceDto> Services { get; set; } = new();
        public ProviderServiceAreaDto ServiceArea { get; set; } = new();
    }

    // Reference DTOs for category/subcategory/city
    public class PCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class SubCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public long CategoryId { get; set; }
    }

    public class CityDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
    }
}

