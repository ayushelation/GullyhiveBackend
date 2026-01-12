using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IPartnerEarningRepository
    {
        Task<IEnumerable<PartnerEarningDto>> GetByUserIdAsync(int userId);
        Task<decimal> GetTotalApprovedEarningsAsync(int userId);
    }
}
