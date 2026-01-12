using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IServiceCategoryMasterService
    {
        Task<IEnumerable<ServiceCategoryMasterDto>> GetAllAsync();
        Task<ServiceCategoryMasterDto> GetByIdAsync(long id);
        Task<long> CreateAsync(ServiceCategoryMasterCreateDto dto);
        Task UpdateAsync(long id, ServiceCategoryMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
