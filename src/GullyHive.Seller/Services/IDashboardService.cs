using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IDashboardService
    {
        Task<SellerDashboardDto> GetDashboardDataAsync(string username);
    }
}
