using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class LeadStatusMasterService : ILeadStatusMasterService
    {
        private readonly ILeadStatusMasterRepository _repo;

        public LeadStatusMasterService(ILeadStatusMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<LeadStatusMasterDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<LeadStatusMasterDto> GetByIdAsync(long id)
        {
            var status = await _repo.GetByIdAsync(id);
            if (status == null)
                throw new KeyNotFoundException("Lead status not found");

            return status;
        }

        public Task<long> CreateAsync(LeadStatusMasterCreateDto dto)
            => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, LeadStatusMasterUpdateDto dto)
            => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id)
            => _repo.DeleteAsync(id);
    }
}
