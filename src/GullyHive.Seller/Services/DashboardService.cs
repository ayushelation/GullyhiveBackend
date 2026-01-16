using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;
using GullyHive.Seller.Services;

namespace GullyHive.Seller.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<SellerDashboardDto> GetDashboardDataAsync(string username)
        {
            var user = await _repo.GetSellerByUsernameAsync(username);
            if (user == null)
                throw new Exception("Seller not found");

            var stats = await _repo.GetSellerStatsAsync(user.Id);
            var recentLeads = await _repo.GetRecentLeadsAsync(user.Id);

            return new SellerDashboardDto
            {
                SellerId = user.Id,
                Name = user.DisplayName,
                Email = user.email,
                Stats = stats,
                RecentLeads = recentLeads,
                ProfilePictureUrl = user.ProfilePictureUrl,
            };
        }
    }
}
