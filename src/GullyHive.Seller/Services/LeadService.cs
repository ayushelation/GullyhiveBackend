using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;
using GullyHive.Seller.Services;

namespace GullyHive.Seller.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        public LeadService(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public async Task<IEnumerable<LeadDto>> GetRecentLeadsAsync()
        {
            // Future business logic goes here
            return await _leadRepository.GetRecentLeadsAsync();
        }
    }

}
