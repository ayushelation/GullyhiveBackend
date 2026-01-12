using Dapper;
using GullyHive.Admin.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace GullyHive.Admin.Repositories
{

    public class StateMasterRepository : IStateMasterRepository
    {
        private readonly string _connectionString;

        public StateMasterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<StateDto>> GetStatesAsync()
        {
            const string sql = @"
                SELECT id,
                       name,
                       is_active AS IsActive,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM state_master
                ORDER BY id DESC";

            using var conn = GetConnection();
            return await conn.QueryAsync<StateDto>(sql);
        }

        public async Task<StateDto?> GetStateByIdAsync(long id)
        {
            const string sql = @"
                SELECT id,
                       name,
                       is_active AS IsActive,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM state_master
                WHERE id = @Id";

            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<StateDto>(sql, new { Id = id });
        }

        public async Task<long> InsertStateAsync(StateCreateDto dto)
        {
            const string sql = @"
                INSERT INTO state_master (name, created_at, updated_at)
                VALUES (@Name, NOW(), NOW())
                RETURNING id";

            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateStateAsync(long id, StateUpdateDto dto)
        {
            const string sql = @"
                UPDATE state_master
                SET name = @Name,
                    is_active = @IsActive,
                    updated_at = NOW()
                WHERE id = @Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                dto.Name,
                dto.IsActive
            });
        }

        public async Task DeleteStateAsync(long id)
        {
            const string sql = @"DELETE FROM state_master WHERE id = @Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }

      

    }
}
