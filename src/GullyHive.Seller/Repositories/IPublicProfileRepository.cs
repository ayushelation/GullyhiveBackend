using GullyHive.Seller.Models;

public interface IPublicProfileRepository
{
    Task<PublicProfileDto?> GetPublicProfileAsync(long providerId);
    Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto);
}

