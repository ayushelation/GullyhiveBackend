using Dapper;
using GullyHive.Admin.Models;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class ProviderStatusMasterRepository : IProviderStatusMasterRepository
    {
        private readonly string _connStr;

        public ProviderStatusMasterRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConn() => new(_connStr);

        public async Task<IEnumerable<ProviderStatusMasterDto>> GetAllAsync()
        {
            const string sql = """
                SELECT id,
                       name,
                       is_active AS IsActive
                FROM provider_status_master
                ORDER BY id;
            """;

            using var conn = GetConn();
            return await conn.QueryAsync<ProviderStatusMasterDto>(sql);
        }

        public async Task<ProviderStatusMasterDto?> GetByIdAsync(long id)
        {
            const string sql = """
                SELECT id,
                       name,
                       is_active AS IsActive
                FROM provider_status_master
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QueryFirstOrDefaultAsync<ProviderStatusMasterDto>(
                sql, new { Id = id });
        }

        public async Task<long> InsertAsync(ProviderStatusMasterCreateDto dto)
        {
            const string sql = """
                INSERT INTO provider_status_master (name)
                VALUES (@Name)
                RETURNING id;
            """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateAsync(long id, ProviderStatusMasterUpdateDto dto)
        {
            const string sql = """
                UPDATE provider_status_master
                SET name = @Name,
                    is_active = @IsActive,
                    updated_at = NOW()
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                dto.Name,
                dto.IsActive
            });
        }

        public async Task DeleteAsync(long id)
        {
            const string sql =
                "DELETE FROM provider_status_master WHERE id = @Id;";

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
