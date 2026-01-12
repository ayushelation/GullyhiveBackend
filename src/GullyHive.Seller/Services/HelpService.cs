using GullyHive.Seller.Models;
using GullyHive.Seller.Repositories;

namespace GullyHive.Seller.Services
{
    public class HelpService : IHelpService
    {
        private readonly IHelpRepository _repo;

        public HelpService(IHelpRepository repo)
        {
            _repo = repo;
        }

        public Task<(List<HelpCategoryDto>, List<HelpFaqDto>)> GetHelpDataAsync()
        {
            return _repo.GetHelpDataAsync();
        }
    }

}
