using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _repo;

        public UserRoleService(IUserRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<long?> AssignRoleAsync(UserRoleCreateDto dto)
        {
            return await _repo.AssignRoleAsync(dto);
        }

        public async Task<(bool Success, string? Message)> UpdateAsync(long id, UserRoleUpdateDto dto)
        {
            return await _repo.UpdateAsync(id, dto);
        }


        public async Task<bool> DeleteAsync(long id)
        {
            return await _repo.DeleteAsync(id);
        }


        public async Task<IEnumerable<UserRoleDto>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(long userId)
        {
            return await _repo.GetRolesByUserIdAsync(userId);
        }

    }
}
