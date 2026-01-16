using Dapper;
using GullyHive.Seller.Models;
using Npgsql;

namespace GullyHive.Seller.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly string _connectionString;

        public ProviderRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<ProviderServicesInitDto> GetProviderServicesInitAsync(long providerId)
        {
            var sql = @"
-- Provider services
SELECT provider_id AS ProviderId, category_id AS CategoryId, sub_category_id AS SubCategoryId
FROM public.provider_service_categories
WHERE provider_id = @ProviderId AND is_active = true;

-- Service area
SELECT type::text AS Type, city_id AS CityId, radius_km AS RadiusKm, pincodes
FROM india_leadgen.provider_service_areas
WHERE provider_id = @ProviderId AND is_active = true;

-- Categories
SELECT id, name FROM india_leadgen.service_categories WHERE is_active = true;

-- Subcategories
SELECT id, name, category_id AS CategoryId FROM india_leadgen.sub_category_master WHERE is_active = true;

-- Cities
SELECT id, name, state FROM india_leadgen.cities WHERE is_active = true;
";

            using var conn = GetConnection();
            using var multi = await conn.QueryMultipleAsync(sql, new { ProviderId = providerId });

            var servicesRaw = (await multi.ReadAsync<(long CategoryId, long SubCategoryId)>()).ToList();
            var serviceAreaList = (await multi.ReadAsync<ProviderServiceAreaDto>()).ToList();
            var categories = (await multi.ReadAsync<PCategoryDto>()).ToList();
            var subCategories = (await multi.ReadAsync<SubCategoryDto>()).ToList();
            var cities = (await multi.ReadAsync<CityDto>()).ToList();

            // Group services by category
            var groupedServices = servicesRaw
                .GroupBy(s => s.CategoryId)
                .Select(g => new ProviderServiceDto
                {
                    CategoryId = g.Key,
                    SubCategoryIds = g.Select(s => s.SubCategoryId).ToList()
                }).ToList();

            return new ProviderServicesInitDto
            {
                ProviderServices = groupedServices,
                ServiceArea = serviceAreaList.FirstOrDefault() ?? new ProviderServiceAreaDto(),
                Categories = categories,
                SubCategories = subCategories,
                Cities = cities
            };
        }

        public async Task UpdateProviderServicesAsync(long providerId, UpdateProviderServicesDto dto)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Delete old services
                await conn.ExecuteAsync(
                    "DELETE FROM public.provider_service_categories WHERE provider_id = @ProviderId",
                    new { ProviderId = providerId },
                    transaction
                );

                // Insert new services
                foreach (var service in dto.Services)
                {
                    foreach (var subId in service.SubCategoryIds)
                    {
                        await conn.ExecuteAsync(@"
INSERT INTO public.provider_service_categories (provider_id, category_id, sub_category_id, is_active, created_at)
VALUES (@ProviderId, @CategoryId, @SubCategoryId, true, now())",
                        new { ProviderId = providerId, CategoryId = service.CategoryId, SubCategoryId = subId },
                        transaction);
                    }
                }

                // Upsert service area
                await conn.ExecuteAsync(@"
INSERT INTO india_leadgen.provider_service_areas (provider_id, type, city_id, radius_km, pincodes, is_active, created_at, updated_at)
VALUES (@ProviderId, @Type, @CityId, @RadiusKm, @Pincodes, true, now(), now())
ON CONFLICT (provider_id) DO UPDATE
SET type = @Type,
    city_id = @CityId,
    radius_km = @RadiusKm,
    pincodes = @Pincodes,
    updated_at = now(),
    is_active = true
",
                    new
                    {
                        ProviderId = providerId,
                        dto.ServiceArea.Type,
                        dto.ServiceArea.CityId,
                        dto.ServiceArea.RadiusKm,
                        Pincodes = dto.ServiceArea.Pincodes
                    },
                    transaction
                );

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
