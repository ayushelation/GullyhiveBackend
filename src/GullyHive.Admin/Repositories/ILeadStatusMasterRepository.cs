using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface ILeadStatusMasterRepository
    {
        Task<IEnumerable<LeadStatusMasterDto>> GetAllAsync();
        Task<LeadStatusMasterDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(LeadStatusMasterCreateDto dto);
        Task UpdateAsync(long id, LeadStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
