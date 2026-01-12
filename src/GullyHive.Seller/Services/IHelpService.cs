using GullyHive.Seller.Models;

namespace GullyHive.Seller.Services
{
    public interface IHelpService
    {
        Task<(List<HelpCategoryDto> Categories, List<HelpFaqDto> Faqs)> GetHelpDataAsync();
    }

}
