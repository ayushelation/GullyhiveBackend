using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class JobStatusMasterService : IJobStatusMasterService
    {
        private readonly IJobStatusMasterRepository _repo;

        public JobStatusMasterService(IJobStatusMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<JobStatusMasterDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<JobStatusMasterDto> GetByIdAsync(long id)
        {
            var status = await _repo.GetByIdAsync(id);
            if (status == null) throw new KeyNotFoundException("Job status not found");
            return status;
        }

        public Task<long> CreateAsync(JobStatusMasterCreateDto dto)
            => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, JobStatusMasterUpdateDto dto)
            => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id)
            => _repo.DeleteAsync(id);
    }
}
