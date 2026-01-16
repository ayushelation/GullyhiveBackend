using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IProviderRepository
    {
        Task<ProviderServicesInitDto> GetProviderServicesInitAsync(long providerId);
        Task UpdateProviderServicesAsync(long providerId, UpdateProviderServicesDto dto);
    }
}
