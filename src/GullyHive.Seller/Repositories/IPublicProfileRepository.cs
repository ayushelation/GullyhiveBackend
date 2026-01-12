using GullyHive.Seller.Models;

public interface IPublicProfileRepository
{
    Task<PublicProfileDto?> GetPublicProfileAsync(long sellerId);
    Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto);
}
