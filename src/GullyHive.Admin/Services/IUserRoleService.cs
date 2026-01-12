using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IUserRoleService
    {
        Task<long?> AssignRoleAsync(UserRoleCreateDto dto);
        Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(long userId);
        Task<IEnumerable<UserRoleDto>> GetAllAsync();   // ✅ NEW
        Task<(bool Success, string? Message)> UpdateAsync(long id, UserRoleUpdateDto dto);
        Task<bool> DeleteAsync(long id);                    // ✅ by ID
    }
}
