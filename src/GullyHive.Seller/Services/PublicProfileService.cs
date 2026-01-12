using GullyHive.Seller.Models;

public class PublicProfileService : IPublicProfileService
{
    private readonly IPublicProfileRepository _repository;

    public PublicProfileService(IPublicProfileRepository repository)
    {
        _repository = repository;
    }

    public Task<PublicProfileDto?> GetPublicProfileAsync(long sellerId)
    {
        return _repository.GetPublicProfileAsync(sellerId);
    }
    public Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto)
    {
        // Optional: add validation logic here
        return _repository.UpdateProfileAsync(providerId, dto);
    }
}
