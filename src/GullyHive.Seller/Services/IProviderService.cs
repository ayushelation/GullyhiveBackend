using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IProviderService
    {
        Task<ProviderServicesInitDto> GetProviderServicesInit(long providerId);
        Task UpdateProviderServices(long providerId, UpdateProviderServicesDto dto);
    }

}
