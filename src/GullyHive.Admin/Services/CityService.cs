using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using GullyHive.Admin.Services;

namespace GullyHive.Admin.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repo;

        public CityService(ICityRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<CityDto>> GetAllCitiesAsync() => _repo.GetAllCitiesAsync();

        public async Task<CityDto?> GetCityByIdAsync(long id) => await _repo.GetCityByIdAsync(id);

        public Task<long> CreateCityAsync(CityCreateDto dto) => _repo.InsertCityAsync(dto);

        public async Task<(bool Success, string Message)> UpdateCityAsync(long id, CityUpdateDto dto)
        {
            var city = await _repo.GetCityByIdAsync(id);
            if (city == null)
                return (false, "City not found");

            await _repo.UpdateCityAsync(id, dto);
            return (true, "City updated successfully");
        }

        public async Task<(bool Success, string Message)> DeleteCityAsync(long id)
        {
            var city = await _repo.GetCityByIdAsync(id);
            if (city == null)
                return (false, "City not found");

            await _repo.DeleteCityAsync(id);
            return (true, "City deleted successfully");
        }
    }
}
