using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface IUserRoleRepository
    {
        Task<long?> AssignRoleAsync(UserRoleCreateDto dto);
        Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(long userId);
        Task<IEnumerable<UserRoleDto>> GetAllAsync();   // ✅ NEW

        Task<(bool Success, string? Message)> UpdateAsync(long id, UserRoleUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }

}
