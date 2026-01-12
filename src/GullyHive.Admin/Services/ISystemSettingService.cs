using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface ISystemSettingService
    {
        Task<IEnumerable<SystemSettingDto>> GetAllAsync();
        Task<SystemSettingDto?> GetByKeyAsync(string key);
        Task<bool> UpsertAsync(SystemSettingCreateUpdateDto dto);
        Task<bool> DeleteAsync(string key);
    }
}
