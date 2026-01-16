using GullyHive.Seller.Models;


public interface IPublicProfileService
{
    Task<PublicProfileDto?> GetPublicProfileAsync(long sellerId);
    Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto);
}