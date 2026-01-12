using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IProviderStatusMasterService
    {
        Task<IEnumerable<ProviderStatusMasterDto>> GetAllAsync();
        Task<ProviderStatusMasterDto> GetByIdAsync(long id);
        Task<long> CreateAsync(ProviderStatusMasterCreateDto dto);
        Task UpdateAsync(long id, ProviderStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
