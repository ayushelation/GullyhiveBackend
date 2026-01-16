using GullyHive.Auth.Models;

public interface IServiceCategoryRepository
{
    Task<IEnumerable<ServiceCategoryDto>> GetParentCategoriesAsync();
    Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(long parentId);
}
