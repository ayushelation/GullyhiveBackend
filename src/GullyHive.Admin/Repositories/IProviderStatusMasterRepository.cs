using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface IProviderStatusMasterRepository
    {
        Task<IEnumerable<ProviderStatusMasterDto>> GetAllAsync();
        Task<ProviderStatusMasterDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(ProviderStatusMasterCreateDto dto);
        Task UpdateAsync(long id, ProviderStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
