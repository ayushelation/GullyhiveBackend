using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface ILeadService
    {
        Task<IEnumerable<LeadDto>> GetRecentLeadsAsync();
    }

}
