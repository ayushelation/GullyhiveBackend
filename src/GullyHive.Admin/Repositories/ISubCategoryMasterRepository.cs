using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface ISubCategoryMasterRepository
    {
        Task<IEnumerable<SubCategoryMasterDto>> GetAllAsync();
        Task<SubCategoryMasterDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(SubCategoryMasterCreateDto dto);
        Task UpdateAsync(long id, SubCategoryMasterUpdateDto dto);
        Task DeleteAsync(long id);
    }
}
