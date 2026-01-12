using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IReferralService
    {
        Task<IEnumerable<ReferralDto>> GetReferralsAsync(int userId);
    }
}
