using Dapper;
using Npgsql;
using GullyHive.Seller.Models;

public class PublicProfileRepository : IPublicProfileRepository
{
    private readonly string _connStr;

    public PublicProfileRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("ConStr")!;
    }

    private NpgsqlConnection GetConnection() => new(_connStr);



    //    public async Task<PublicProfileDto?> GetPublicProfileAsync(long providerId)
    //    {
    //        var sql = @"
    //SELECT 
    //    p.user_id AS SellerId,
    //    p.legal_name AS LegalName,
    //    u.display_name AS DisplayName,
    //    u.email AS Email,
    //    u.phone AS Phone,
    //    p.provider_type::text AS ProviderType,
    //    p.status::text AS Status,
    //    c.name AS BaseCity,
    //    p.description AS Description,
    //    p.avg_rating AS AvgRating,
    //    p.rating_count AS RatingCount,
    //    p.total_jobs_completed AS TotalJobsCompleted,
    //    p.total_disputes AS TotalDisputes,
    //    p.dispute_rate AS DisputeRate,

    //   -- Address (primary)
    //    a.id        AS AddressId,
    //    a.label     AS AddressLabel,
    //    a.line1     AS AddressLine1,
    //    a.line2     AS AddressLine2,
    //    a.locality  AS Locality,
    //    ac.name     AS AddressCity,
    //    a.state     AS State,
    //    a.pincode   AS Pincode



    //FROM india_leadgen.provider_profiles p
    //JOIN india_leadgen.users u 
    //    ON u.id = p.user_id
    //LEFT JOIN india_leadgen.cities c 
    //    ON c.id = p.base_city_id
    //LEFT JOIN india_leadgen.addresses a 
    //    ON a.user_id = u.id 
    //   AND a.is_primary = true
    //LEFT JOIN india_leadgen.cities ac
    //    ON ac.id = a.city_id
    //WHERE p.user_id = @ProviderId;
    //";


    //        await using var conn = GetConnection();
    //        var result = await conn.QueryFirstOrDefaultAsync<PublicProfileDto>(sql, new { ProviderId = providerId });

    //        return result;
    //    }
    public async Task<PublicProfileDto?> GetPublicProfileAsync(long providerId)
    {
        var sql = @"
WITH base AS (
    SELECT 
        p.user_id AS SellerId,
        p.legal_name AS LegalName,
        u.display_name AS DisplayName,
        u.email AS Email,
        u.phone AS Phone,
        p.provider_type::text AS ProviderType,
        p.status::text AS Status,
        c.name AS BaseCity,
        c.state AS State,
        p.description AS Description,
        p.avg_rating AS AvgRating,
        p.rating_count AS RatingCount,
        p.total_jobs_completed AS TotalJobsCompleted,
        p.total_disputes AS TotalDisputes,
        p.dispute_rate AS DisputeRate,
        a.id AS AddressId,
        a.label AS AddressLabel,
        a.line1 AS AddressLine1,
        a.line2 AS AddressLine2,
        a.locality AS Locality,
        ac.name AS AddressCity,
        a.state AS State,
        a.pincode AS Pincode
    FROM india_leadgen.provider_profiles p
    JOIN india_leadgen.users u ON u.id = p.user_id
    LEFT JOIN india_leadgen.cities c ON c.id = p.base_city_id
    LEFT JOIN india_leadgen.addresses a ON a.user_id = u.id AND a.is_primary = true
    LEFT JOIN india_leadgen.cities ac ON ac.id = a.city_id
    WHERE p.user_id = @ProviderId
)
SELECT 
    b.*,
    COALESCE(s.services, '[]') AS Services,
    COALESCE(pf.portfolio, '[]') AS PortfolioImages,
    COALESCE(r.reviews, '[]') AS Reviews
FROM base b
LEFT JOIN (
    SELECT ps.provider_id, JSON_AGG(sc.name) AS services
    FROM india_leadgen.provider_service_areas ps
    JOIN india_leadgen.service_categories sc ON sc.id = ps.provider_id
    WHERE ps.provider_id = @ProviderId
    GROUP BY ps.provider_id
) s ON s.provider_id = b.SellerId
LEFT JOIN (
    SELECT provider_id, JSON_AGG(file_url) AS portfolio
    FROM india_leadgen.provider_documents
    WHERE provider_id = @ProviderId
    GROUP BY provider_id
) pf ON pf.provider_id = b.SellerId
LEFT JOIN (
    SELECT provider_id, JSON_AGG(
        JSON_BUILD_OBJECT(
            'ReviewerName', u.display_name,
            'Rating', r.rating,
            'Comment', r.comment,
            'CreatedAt', r.created_at
        )
    ) AS reviews
    FROM india_leadgen.reviews r
    JOIN india_leadgen.users u ON u.id = r.reviewer_user_id
    WHERE r.provider_id = @ProviderId
    GROUP BY provider_id
) r ON r.provider_id = b.SellerId;
";

        await using var conn = GetConnection();
        var result = await conn.QueryFirstOrDefaultAsync<PublicProfileDto>(sql, new { ProviderId = providerId });
        return result;
    }


    public async Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto)
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();

        using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // 1. Update user table
            var userSql = @"
UPDATE india_leadgen.users
SET display_name = @DisplayName,
    email = @Email,
    phone = @Phone
WHERE id = @ProviderId;";

            await conn.ExecuteAsync(userSql, new
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                Phone = dto.Phone,
                ProviderId = providerId
            }, transaction);

            // 2. Update provider_profiles description
            var providerSql = @"
UPDATE india_leadgen.provider_profiles
SET description = @Description
WHERE user_id = @ProviderId;";

            await conn.ExecuteAsync(providerSql, new
            {
                Description = dto.Description,
                ProviderId = providerId
            }, transaction);

            // 3. Update primary address
            var addressSql = @"
UPDATE india_leadgen.addresses
SET line1 = @AddressLine1,
    state = @State,
    pincode = @Pincode,
    city_id = (SELECT id FROM india_leadgen.cities WHERE name = @City LIMIT 1)
WHERE user_id = @ProviderId AND is_primary = true;";

            await conn.ExecuteAsync(addressSql, new
            {
                AddressLine1 = dto.AddressLine1,
                State = dto.State,
                Pincode = dto.Pincode,
                City = dto.City,
                ProviderId = providerId
            }, transaction);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
