using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> GetByIdAsync(long id);
        Task<long> CreateAsync(RoleCreateDto dto);
        Task UpdateAsync(long id, RoleUpdateDto dto);
        Task DeleteAsync(long id);
        Task<bool> CheckTableAsync(string tableName);
    }

}
