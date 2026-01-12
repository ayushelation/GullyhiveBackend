using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IPartnerEarningService
    {
        Task<IEnumerable<PartnerEarningDto>> GetEarningsAsync(int userId);
        Task<decimal> GetTotalEarningsAsync(int userId);
    }
}
