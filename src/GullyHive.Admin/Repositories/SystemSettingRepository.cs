using Dapper;
using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly string _connectionString;

        public SystemSettingRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<SystemSettingDto>> GetAllAsync()
        {
            const string sql = @"
                SELECT key, value, updated_at AS UpdatedAt
                FROM system_settings
                ORDER BY key;
            ";

            using var conn = GetConnection();
            return await conn.QueryAsync<SystemSettingDto>(sql);
        }

        public async Task<SystemSettingDto?> GetByKeyAsync(string key)
        {
            const string sql = @"
                SELECT key, value, updated_at AS UpdatedAt
                FROM system_settings
                WHERE key = @Key;
            ";

            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<SystemSettingDto>(sql, new { Key = key });
        }

        // INSERT or UPDATE (SAFE for duplicates)
        public async Task<bool> UpsertAsync(SystemSettingCreateUpdateDto dto)
        {
            const string sql = @"
                INSERT INTO system_settings (key, value, updated_at)
                VALUES (@Key, @Value, NOW())
                ON CONFLICT (key)
                DO UPDATE SET
                    value = EXCLUDED.value,
                    updated_at = NOW();
            ";

            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, dto) > 0;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            const string sql = "DELETE FROM system_settings WHERE key = @Key;";
            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, new { Key = key }) > 0;
        }
    }
}
