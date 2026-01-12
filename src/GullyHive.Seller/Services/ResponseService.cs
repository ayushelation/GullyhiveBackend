using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;
using GullyHive.Seller.Services;

namespace GullyHive.Seller.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _repo;

        public ResponseService(IResponseRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ResponseDto>> GetMyResponses(long sellerId)
        {
            var responses = await _repo.GetByUserAsync(sellerId);
            return responses.ToList();
        }

        public Task<ResponseDto?> GetResponse(long id, long sellerId)
            => _repo.GetByIdAsync(id, sellerId);

        public Task<long> Create(long sellerId, CreateResponseDto dto)
            => _repo.CreateAsync(sellerId, dto);

        public Task<bool> Update(long id, long sellerId, UpdateResponseDto dto)
            => _repo.UpdateAsync(id, sellerId, dto);

        public Task<bool> UpdateStatus(long id, long sellerId, string status)
            => _repo.UpdateStatusAsync(id, sellerId, status);

        public Task<bool> Delete(long id, long sellerId)
            => _repo.DeleteAsync(id, sellerId);
    }
}


 