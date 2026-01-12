using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IReferralRepository
    {
        Task<IEnumerable<ReferralDto>> GetByUserIdAsync(int userId);
    }
}
