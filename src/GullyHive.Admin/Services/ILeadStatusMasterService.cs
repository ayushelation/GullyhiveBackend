using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface ILeadStatusMasterService
    {
        Task<IEnumerable<LeadStatusMasterDto>> GetAllAsync();
        Task<LeadStatusMasterDto> GetByIdAsync(long id);
        Task<long> CreateAsync(LeadStatusMasterCreateDto dto);
        Task UpdateAsync(long id, LeadStatusMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
