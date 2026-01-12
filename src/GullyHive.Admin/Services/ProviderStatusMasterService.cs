using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class ProviderStatusMasterService : IProviderStatusMasterService
    {
        private readonly IProviderStatusMasterRepository _repo;

        public ProviderStatusMasterService(IProviderStatusMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<ProviderStatusMasterDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<ProviderStatusMasterDto> GetByIdAsync(long id)
        {
            var status = await _repo.GetByIdAsync(id);
            if (status == null)
                throw new KeyNotFoundException("Provider status not found");

            return status;
        }

        public Task<long> CreateAsync(ProviderStatusMasterCreateDto dto)
            => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, ProviderStatusMasterUpdateDto dto)
            => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id)
            => _repo.DeleteAsync(id);
    }
}
