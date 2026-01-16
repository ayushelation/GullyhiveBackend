

using Dapper;
using Npgsql;
using Newtonsoft.Json;
using GullyHive.Seller.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

public class PublicProfileRepository : IPublicProfileRepository
{
    private readonly string _connStr;

    public PublicProfileRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("ConStr")!;
    }

    private NpgsqlConnection GetConnection() => new(_connStr);






    public async Task<PublicProfileDto?> GetPublicProfileAsync(long sellerId)
    {
        var sql = @"
WITH base AS (
    SELECT 
        p.id AS ""ProviderId"",
        p.user_id AS ""UserId"",
        p.legal_name AS ""LegalName"",
        u.display_name AS ""DisplayName"",
        u.email AS ""Email"",
        u.phone AS ""Phone"",
        p.provider_type::text AS ""ProviderType"",
        p.status::text AS ""Status"",
        c.name AS ""BaseCity"",
        c.state AS ""State"",
        p.description AS ""Description"",
        p.created_at AS ""CreatedAt"",
        p.profile_picture_url AS ""ProfilePictureUrl"",
        p.total_jobs_completed AS ""TotalJobsCompleted"",
        p.total_disputes AS ""TotalDisputes"",
        p.dispute_rate AS ""DisputeRate"",
        a.id AS ""AddressId"",
        a.label AS ""AddressLabel"",
        a.line1 AS ""AddressLine1"",
        a.line2 AS ""AddressLine2"",
        a.locality AS ""Locality"",
        ac.name AS ""AddressCity"",
        a.state AS ""State"",
        a.pincode AS ""Pincode""
    FROM india_leadgen.provider_profiles p
    JOIN india_leadgen.users u ON u.id = p.user_id
    LEFT JOIN india_leadgen.cities c ON c.id = p.base_city_id
    LEFT JOIN india_leadgen.addresses a ON a.user_id = u.id AND a.is_primary = true
    LEFT JOIN india_leadgen.cities ac ON ac.id = a.city_id
    WHERE p.user_id = @SellerId
)

SELECT
    b.*,
    COALESCE(ratings.""AvgRating"", 0)::numeric(3,2) AS ""AvgRating"",
    COALESCE(ratings.""RatingCount"", 0) AS ""RatingCount"",
    COALESCE(s.services, '[]') AS ""ServicesJson"",
    COALESCE(pf.portfolio, '[]') AS ""PortfolioJson"",
    COALESCE(r.reviews, '[]') AS ""ReviewsJson""
FROM base b

-- Aggregate ratings dynamically
LEFT JOIN (
    SELECT 
        provider_id,
        AVG(rating)::numeric(3,2) AS ""AvgRating"",
        COUNT(*) AS ""RatingCount""
    FROM india_leadgen.reviews
    GROUP BY provider_id
) ratings ON ratings.provider_id = b.""ProviderId""

-- Services
LEFT JOIN (
    SELECT psc.provider_id,
           COALESCE(JSON_AGG(sc.name || ' - ' || ssc.name ORDER BY sc.name) FILTER (WHERE sc.name IS NOT NULL), '[]') AS services
    FROM public.provider_service_categories psc
    LEFT JOIN india_leadgen.service_categories sc ON sc.id = psc.category_id
    LEFT JOIN india_leadgen.sub_category_master ssc ON ssc.id = psc.sub_category_id
    WHERE psc.is_active = true
    GROUP BY psc.provider_id
) s ON s.provider_id = b.""UserId""

-- Portfolio
LEFT JOIN (
    SELECT provider_id, COALESCE(JSON_AGG(file_url), '[]') AS portfolio
    FROM india_leadgen.provider_documents
    GROUP BY provider_id
) pf ON pf.provider_id = b.""ProviderId""

-- Reviews
LEFT JOIN (
    SELECT r.provider_id, COALESCE(
        JSON_AGG(
            JSON_BUILD_OBJECT(
                'ReviewerName', u.display_name,
                'Rating', r.rating,
                'Comment', r.comment,
                'CreatedAt', r.created_at
            )
        ), '[]'
    ) AS reviews
    FROM india_leadgen.reviews r
    JOIN india_leadgen.users u ON u.id = r.reviewer_user_id
    GROUP BY r.provider_id
) r ON r.provider_id = b.""ProviderId"";



";

        try
        {
            await using var conn = GetConnection();
            var raw = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new { SellerId = sellerId });

            if (raw == null) return null;

            // Deserialize JSON arrays safely
            var services = System.Text.Json.JsonSerializer.Deserialize<List<string>>(Convert.ToString(raw.ServicesJson) ?? "[]") ?? new List<string>();
            var portfolio = System.Text.Json.JsonSerializer.Deserialize<List<string>>(Convert.ToString(raw.PortfolioJson) ?? "[]") ?? new List<string>();
            var reviews = System.Text.Json.JsonSerializer.Deserialize<List<ReviewDto>>(Convert.ToString(raw.ReviewsJson) ?? "[]") ?? new List<ReviewDto>();

            return new PublicProfileDto
            {
                ProviderId = raw.ProviderId,
                SellerId = raw.UserId,
                LegalName = raw.LegalName,
                DisplayName = raw.DisplayName,
                ProfilePictureUrl = raw.ProfilePictureUrl,
                Email = raw.Email,
                Phone = raw.Phone,
                ProviderType = raw.ProviderType,
                Status = raw.Status,
                BaseCity = raw.BaseCity,
                State = raw.State,
                Description = raw.Description,
                AvgRating = raw.AvgRating,
                RatingCount = raw.RatingCount,
                TotalJobsCompleted = raw.TotalJobsCompleted,
                TotalDisputes = raw.TotalDisputes,
                DisputeRate = raw.DisputeRate,
                AddressId = raw.AddressId as long?,
                AddressLabel = raw.AddressLabel,
                AddressLine1 = raw.AddressLine1,
                AddressLine2 = raw.AddressLine2,
                Locality = raw.Locality,
                AddressCity = raw.AddressCity,
                Pincode = raw.Pincode,
                Services = services,
                PortfolioImages = portfolio,
                Reviews = reviews,
                CreatedAt= raw.CreatedAt
            };
        }
        catch (PostgresException pgEx)
        {
            Console.WriteLine($"PostgreSQL error: {pgEx.MessageText}");
            throw;
        }
        catch (System.Text.Json.JsonException jsonEx)
        {
            Console.WriteLine($"JSON parse error: {jsonEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }





    //    public async Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto)
    //    {
    //        await using var conn = GetConnection();
    //        await conn.OpenAsync();

    //        await using var transaction = await conn.BeginTransactionAsync();

    //        try
    //        {
    //            // Update user
    //            var userSql = @"
    //UPDATE india_leadgen.users
    //SET display_name = @DisplayName,
    //    email = @Email,
    //    phone = @Phone
    //WHERE id = @ProviderId;";
    //            await conn.ExecuteAsync(userSql, new { dto.DisplayName, dto.Email, dto.Phone, ProviderId = providerId }, transaction);

    //            // Update provider description
    //            var providerSql = @"
    //UPDATE india_leadgen.provider_profiles
    //SET description = @Description
    //WHERE user_id = @ProviderId;";
    //            await conn.ExecuteAsync(providerSql, new { dto.Description, ProviderId = providerId }, transaction);

    //            // Update primary address
    //            var addressSql = @"
    //UPDATE india_leadgen.addresses
    //SET line1 = @AddressLine1,
    //    state = @State,
    //    pincode = @Pincode,
    //    city_id = (SELECT id FROM india_leadgen.cities WHERE name = @City LIMIT 1)
    //WHERE user_id = @ProviderId AND is_primary = true;";
    //            await conn.ExecuteAsync(addressSql, new { dto.AddressLine1, dto.State, dto.Pincode, City = dto.City, ProviderId = providerId }, transaction);

    //            await transaction.CommitAsync();
    //            return true;
    //        }
    //        catch
    //        {
    //            await transaction.RollbackAsync();
    //            return false;
    //        }
    //    }
    public async Task<bool> UpdateProfileAsync(long providerId, UpdateProfileDto dto)
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();

        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // --- 1. Update user info ---
            var userSql = @"
UPDATE india_leadgen.users
SET display_name = @DisplayName,
    email = @Email,
    phone = @Phone
WHERE id = @ProviderId;";
            await conn.ExecuteAsync(userSql, new { dto.DisplayName, dto.Email, dto.Phone, ProviderId = providerId }, transaction);

            // --- 2. Update provider description ---
            var providerSql = @"
UPDATE india_leadgen.provider_profiles
SET description = @Description
WHERE user_id = @ProviderId;";
            await conn.ExecuteAsync(providerSql, new { dto.Description, ProviderId = providerId }, transaction);

            // --- 3. Update primary address ---
            var addressSql = @"
UPDATE india_leadgen.addresses
SET line1 = @AddressLine1,
    state = @State,
    pincode = @Pincode,
    city_id = (SELECT id FROM india_leadgen.cities WHERE name = @City LIMIT 1)
WHERE user_id = @ProviderId AND is_primary = true;";
            await conn.ExecuteAsync(addressSql, new { dto.AddressLine1, dto.State, dto.Pincode, City = dto.City, ProviderId = providerId }, transaction);

            // --- 4. Update profile picture ---
            if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
            {
                // Define the uploads folder path (like registration)
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profiles");

                // Create directory if not exists
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                // Generate unique filename
                var fileName = $"profile_{providerId}{Path.GetExtension(dto.ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                // Save file
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProfilePicture.CopyToAsync(stream);
                }

                // Save the relative URL in DB (for web access)
                var fileUrl = $"/uploads/profiles/{fileName}";
                var picSql = @"
UPDATE india_leadgen.provider_profiles
SET profile_picture_url = @Url
WHERE user_id = @ProviderId;";
                await conn.ExecuteAsync(picSql, new { Url = fileUrl, ProviderId = providerId }, transaction);
            }

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

