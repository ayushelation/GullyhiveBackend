using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class ServiceCategoryMasterService : IServiceCategoryMasterService
    {
        private readonly IServiceCategoryMasterRepository _repo;

        public ServiceCategoryMasterService(IServiceCategoryMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<ServiceCategoryMasterDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<ServiceCategoryMasterDto> GetByIdAsync(long id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Service category not found");

            return category;
        }

        public Task<long> CreateAsync(ServiceCategoryMasterCreateDto dto)
            => _repo.InsertAsync(dto);

        public Task UpdateAsync(long id, ServiceCategoryMasterUpdateDto dto)
            => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(long id)
            => _repo.DeleteAsync(id);

    }
}