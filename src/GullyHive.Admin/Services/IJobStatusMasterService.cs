using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IJobStatusMasterService
    {
        Task<IEnumerable<JobStatusMasterDto>> GetAllAsync();
        Task<JobStatusMasterDto> GetByIdAsync(long id);
        Task<long> CreateAsync(JobStatusMasterCreateDto dto);
        Task UpdateAsync(long id, JobStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}