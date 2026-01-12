using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface IJobStatusMasterRepository
    {
        Task<IEnumerable<JobStatusMasterDto>> GetAllAsync();
        Task<JobStatusMasterDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(JobStatusMasterCreateDto dto);
        Task UpdateAsync(long id, JobStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
