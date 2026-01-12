using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface ISystemSettingRepository
    {
        Task<IEnumerable<SystemSettingDto>> GetAllAsync();
        Task<SystemSettingDto?> GetByKeyAsync(string key);
        Task<bool> UpsertAsync(SystemSettingCreateUpdateDto dto);
        Task<bool> DeleteAsync(string key);
    }
}
