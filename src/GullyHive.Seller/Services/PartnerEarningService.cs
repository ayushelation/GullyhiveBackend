using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;

namespace GullyHive.Seller.Services
{
    public class PartnerEarningService : IPartnerEarningService
    {
        private readonly IPartnerEarningRepository _repo;

        public PartnerEarningService(IPartnerEarningRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PartnerEarningDto>> GetEarningsAsync(int userId)
        {
            var earnings = await _repo.GetByUserIdAsync(userId);
            return earnings.Select(e => new PartnerEarningDto
            {
                Id = e.Id,
                PartnerUserId = e.PartnerUserId,
                ReferralId = e.ReferralId,
                RuleId = e.RuleId,
                EarningType = e.EarningType,
                Amount = e.Amount,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            });
        }

        public async Task<decimal> GetTotalEarningsAsync(int userId)
        {
            return await _repo.GetTotalApprovedEarningsAsync(userId);
        }
    }
}
