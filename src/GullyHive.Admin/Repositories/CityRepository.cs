using Dapper;
using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
        public class CityRepository : ICityRepository
        {
            private readonly string _connectionString;

            public CityRepository(IConfiguration config)
            {
                _connectionString = config.GetConnectionString("ConStr")!;
            }

            private NpgsqlConnection GetConnection() => new(_connectionString);

            public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
            {
                const string sql = @"
                SELECT id, name, state, country, tier, center_lat AS CenterLat, center_long AS CenterLong,
                       is_active AS IsActive, created_at AS CreatedAt, updated_at AS UpdatedAt
                FROM cities
                ORDER BY id DESC";

                using var conn = GetConnection();
                return await conn.QueryAsync<CityDto>(sql);
            }

            public async Task<CityDto?> GetCityByIdAsync(long id)
            {
                const string sql = @"
                SELECT id, name, state, country, tier, center_lat AS CenterLat, center_long AS CenterLong,
                       is_active AS IsActive, created_at AS CreatedAt, updated_at AS UpdatedAt
                FROM cities
                WHERE id = @Id";

                using var conn = GetConnection();
                return await conn.QueryFirstOrDefaultAsync<CityDto>(sql, new { Id = id });
            }

        public async Task<long> InsertCityAsync(CityCreateDto dto)
        {
            const string sql = @"
            INSERT INTO cities (name, state, country, tier, center_lat, center_long, created_at, updated_at)
            VALUES (@Name, @State, @Country, @Tier, @CenterLat, @CenterLong, NOW(), NOW())
            RETURNING id";

            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }





        public async Task UpdateCityAsync(long id, CityUpdateDto dto)
        {
            const string sql = @"
                UPDATE cities
                SET name = @Name,
                    state = @State,
                    country = @Country,
                    tier = @Tier,
                    center_lat = @CenterLat,
                    center_long = @CenterLong,
                    is_active = @IsActive,
                    updated_at = NOW()
                WHERE id = @Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, new { Id = id, dto.Name, dto.State, dto.Country, dto.Tier, dto.CenterLat, dto.CenterLong, dto.IsActive });
        }


        public async Task DeleteCityAsync(long id)
            {
                const string sql = "DELETE FROM cities WHERE id = @Id";
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
}
