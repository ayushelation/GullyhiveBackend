using GullyHive.Admin.Models;

namespace GuGullyHive.Admin.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(RoleCreateDto dto);
        Task UpdateAsync(long id, RoleUpdateDto dto);
        Task DeleteAsync(long id);
        Task<bool> TableExistsAsync(string tableName, string schema = "public");
    }
}
