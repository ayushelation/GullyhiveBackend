using Dapper;
using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly string _connectionString;

        public UserRoleRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<UserRoleDto>> GetAllAsync()
        {
            const string sql = @"
        SELECT
            ur.id,
            ur.user_id AS UserId,
            ur.role_id AS RoleId,
            r.name AS RoleName,
            ur.created_at AS CreatedAt
        FROM user_roles ur
        JOIN roles r ON r.id = ur.role_id
        ORDER BY ur.created_at DESC;
    ";

            using var conn = GetConnection();
            return await conn.QueryAsync<UserRoleDto>(sql);
        }

        public async Task<long?> AssignRoleAsync(UserRoleCreateDto dto)
        {
            const string sql = @"
        INSERT INTO user_roles (user_id, role_id)
        VALUES (@UserId, @RoleId)
        ON CONFLICT (user_id, role_id) DO NOTHING
        RETURNING id;
    ";

            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<long?>(sql, dto);
        }



        // READ (roles for a user)
        public async Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(long userId)
        {
            const string sql = @"
                SELECT
                    ur.id,
                    ur.user_id AS UserId,
                    ur.role_id AS RoleId,
                    r.name AS RoleName,
                    ur.created_at AS CreatedAt
                FROM user_roles ur
                JOIN roles r ON r.id = ur.role_id
                WHERE ur.user_id = @UserId
                ORDER BY ur.created_at DESC;
            ";

            using var conn = GetConnection();
            return await conn.QueryAsync<UserRoleDto>(sql, new { UserId = userId });
        }
        public async Task<(bool Success, string? Message)> UpdateAsync(long id, UserRoleUpdateDto dto)
        {
            const string sql = @"
        UPDATE user_roles
        SET
            user_id = COALESCE(@UserId, user_id),
            role_id = COALESCE(@RoleId, role_id)
        WHERE id = @Id;
    ";

            using var conn = GetConnection();

            try
            {
                var rows = await conn.ExecuteAsync(sql, new
                {
                    Id = id,
                    dto.UserId,
                    dto.RoleId
                });

                if (rows == 0)
                    return (false, "Record not found.");

                return (true, null);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return (false, "This user already has this role.");
            }
        }



        //Delete
        public async Task<bool> DeleteAsync(long id)
        {
            const string sql = @"
        DELETE FROM user_roles
        WHERE id = @Id;
    ";

            using var conn = GetConnection();
            return (await conn.ExecuteAsync(sql, new { Id = id })) > 0;
        }


    }
}
