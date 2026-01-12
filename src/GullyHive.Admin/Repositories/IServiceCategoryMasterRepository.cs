using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface IServiceCategoryMasterRepository
    {
        Task<IEnumerable<ServiceCategoryMasterDto>> GetAllAsync();
        Task<ServiceCategoryMasterDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(ServiceCategoryMasterCreateDto dto);
        Task UpdateAsync(long id, ServiceCategoryMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
