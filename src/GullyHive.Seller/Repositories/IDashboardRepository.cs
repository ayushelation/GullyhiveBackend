using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IDashboardRepository
    {
        Task<SellerDto?> GetSellerByUsernameAsync(string username);
        Task<SellerStatsDto> GetSellerStatsAsync(long sellerId);
        Task<IEnumerable<LeadDto>> GetRecentLeadsAsync(long sellerId);
    }
}
