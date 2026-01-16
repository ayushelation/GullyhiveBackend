using GullyHive.Auth.Models;

public interface IServiceCategoryService
{
    Task<IEnumerable<ServiceCategoryDto>> GetServicesAsync();
    Task<IEnumerable<SubCategoryDto>> GetCategoriesAsync(long parentId);
}
