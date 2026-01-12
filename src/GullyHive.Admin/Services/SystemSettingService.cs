using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingRepository _repo;

        public SystemSettingService(ISystemSettingRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<SystemSettingDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<SystemSettingDto?> GetByKeyAsync(string key)
            => _repo.GetByKeyAsync(key);

        public Task<bool> UpsertAsync(SystemSettingCreateUpdateDto dto)
            => _repo.UpsertAsync(dto);

        public Task<bool> DeleteAsync(string key)
            => _repo.DeleteAsync(key);
    }
}
