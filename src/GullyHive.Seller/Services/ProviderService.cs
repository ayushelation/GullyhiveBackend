using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;

namespace GullyHive.Seller.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _repo;

        public ProviderService(IProviderRepository repo)
        {
            _repo = repo;
        }

        public Task<ProviderServicesInitDto> GetProviderServicesInit(long providerId)
        {
            return _repo.GetProviderServicesInitAsync(providerId);
        }

        public Task UpdateProviderServices(long providerId, UpdateProviderServicesDto dto)
        {
            return _repo.UpdateProviderServicesAsync(providerId, dto);
        }
    }
}
