using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class SubCategoryMasterService : ISubCategoryMasterService
    {
        private readonly ISubCategoryMasterRepository _repo;

        public SubCategoryMasterService(ISubCategoryMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<SubCategoryMasterDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<SubCategoryMasterDto> GetByIdAsync(long id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                throw new KeyNotFoundException("Sub category not found");

            return data;
        }

        public Task<long> CreateAsync(SubCategoryMasterCreateDto dto)
            => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, SubCategoryMasterUpdateDto dto)
            => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id)
            => _repo.DeleteAsync(id);
    }
}
