using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;

namespace GullyHive.Seller.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _repo;

        public ReferralService(IReferralRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReferralDto>> GetReferralsAsync(int userId)
        {
            var referrals = await _repo.GetByUserIdAsync(userId);

            return referrals.Select(r => new ReferralDto
            {
                Id = r.Id,
                Code = r.Code,
                ReferrerUserId = r.ReferrerUserId,
                ReferrerRole = r.ReferrerRole,
                ReferredType = r.ReferredType,
                ReferredUserId = r.ReferredUserId,
                Source = r.Source,
                CreatedAt = r.CreatedAt,
                Name = r.Name,
                Avatar = !string.IsNullOrEmpty(r.Avatar) ? r.Avatar : $"U{r.ReferredUserId}",
                JoinedDate = r.JoinedDate,
                Status = r.Status,
                Earnings = $"${r.Earnings}",       // formatted string
                Amount = r.Earnings.ToString()     // numeric value as string
            });
        }

    }
}
