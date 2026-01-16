using GullyHive.Auth.Models;
using GullyHive.Auth.Services;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly IServiceCategoryRepository _repo;

    public ServiceCategoryService(IServiceCategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ServiceCategoryDto>> GetServicesAsync()
    {
        return await _repo.GetParentCategoriesAsync();
    }

    public async Task<IEnumerable<SubCategoryDto>> GetCategoriesAsync(long parentId)
    {
        return await _repo.GetSubCategoriesAsync(parentId);
    }
}
