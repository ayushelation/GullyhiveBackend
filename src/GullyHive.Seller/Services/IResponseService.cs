using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IResponseService
    {
        Task<List<ResponseDto>> GetMyResponses(long sellerId);
        Task<ResponseDto?> GetResponse(long id, long sellerId);
        Task<long> Create(long sellerId, CreateResponseDto dto);
        Task<bool> Update(long id, long sellerId, UpdateResponseDto dto);
        Task<bool> UpdateStatus(long id, long sellerId, string status);
        Task<bool> Delete(long id, long sellerId);
    }

}
