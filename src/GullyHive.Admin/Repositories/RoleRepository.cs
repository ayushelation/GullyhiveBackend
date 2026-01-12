using Dapper;
using GuGullyHive.Admin.Repositories;
using GullyHive.Admin.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace GullyHive.Admin.Repositories
{  
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            const string sql = @"
                SELECT id, name, created_at AS CreatedAt
                FROM roles
                ORDER BY id DESC";
            using var conn = GetConnection();
            return await conn.QueryAsync<RoleDto>(sql);
        }

        public async Task<RoleDto?> GetByIdAsync(long id)
        {
            const string sql = @"
                SELECT id, name, created_at AS CreatedAt
                FROM roles
                WHERE id = @Id";
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<RoleDto>(sql, new { Id = id });
        }

        public async Task<long> InsertAsync(RoleCreateDto dto)
        {
            const string sql = @"
                INSERT INTO roles (name)
                VALUES (@Name)
                RETURNING id";
            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateAsync(long id, RoleUpdateDto dto)
        {
            const string sql = @"
                UPDATE roles
                SET name = @Name
                WHERE id = @Id";
            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, new { Id = id, dto.Name });
        }

        public async Task DeleteAsync(long id)
        {
            const string sql = @"DELETE FROM roles WHERE id = @Id";
            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> TableExistsAsync(string tableName, string schema = "india_leadgen,")
        {
            const string sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM information_schema.tables
                    WHERE table_schema = @Schema
                      AND table_name = @TableName
                )";
            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<bool>(sql, new { Schema = schema, TableName = tableName });
        }
    }
}
