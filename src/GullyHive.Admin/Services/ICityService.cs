using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface ICityService
    {
        Task<IEnumerable<CityDto>> GetAllCitiesAsync();
        Task<CityDto?> GetCityByIdAsync(long id);
        Task<long> CreateCityAsync(CityCreateDto dto);
        Task<(bool Success, string Message)> UpdateCityAsync(long id, CityUpdateDto dto);
        Task<(bool Success, string Message)> DeleteCityAsync(long id);
    }
}
