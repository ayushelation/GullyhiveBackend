using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<CityDto>> GetAllCitiesAsync();
        Task<CityDto?> GetCityByIdAsync(long id);
        Task<long> InsertCityAsync(CityCreateDto dto);
        Task UpdateCityAsync(long id, CityUpdateDto dto);
        Task DeleteCityAsync(long id);
    }
}