using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface ILeadRepository
    {
        Task<IEnumerable<LeadDto>> GetRecentLeadsAsync();
    }
}
