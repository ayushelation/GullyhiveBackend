using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IResponseRepository
    {
        Task<IEnumerable<ResponseDto>> GetByUserAsync(long userId);
        Task<ResponseDto?> GetByIdAsync(long id, long userId);
        Task<long> CreateAsync(long userId, CreateResponseDto dto);
        Task<bool> UpdateAsync(long id, long userId, UpdateResponseDto dto);
        Task<bool> UpdateStatusAsync(long id, long userId, string status);
        Task<bool> DeleteAsync(long id, long userId);
    }
}



