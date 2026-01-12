using GuGullyHive.Admin.Repositories;
using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;

        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<RoleDto>> GetAllAsync() => _repo.GetAllAsync();

        public async Task<RoleDto> GetByIdAsync(long id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null)
                throw new KeyNotFoundException("Role not found");
            return role;
        }

        public Task<long> CreateAsync(RoleCreateDto dto) => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, RoleUpdateDto dto) => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id) => _repo.DeleteAsync(id);

        public Task<bool> CheckTableAsync(string tableName) => _repo.TableExistsAsync(tableName);
    }
}
