using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public interface IHelpRepository
    {
        Task<(List<HelpCategoryDto> Categories, List<HelpFaqDto> Faqs)> GetHelpDataAsync();
    }

}
