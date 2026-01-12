using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface ISubCategoryMasterService
    {
        Task<IEnumerable<SubCategoryMasterDto>> GetAllAsync();
        Task<SubCategoryMasterDto> GetByIdAsync(long id);
        Task<long> CreateAsync(SubCategoryMasterCreateDto dto);
        Task UpdateAsync(long id, SubCategoryMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
