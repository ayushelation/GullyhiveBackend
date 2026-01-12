using Dapper;
using GullyHive.Admin.Models;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class SubCategoryMasterRepository : ISubCategoryMasterRepository
    {
        private readonly string _connStr;

        public SubCategoryMasterRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConn() => new(_connStr);

        public async Task<IEnumerable<SubCategoryMasterDto>> GetAllAsync()
        {
            const string sql = """
                SELECT id,
                       category_id AS CategoryId,
                       name,
                       is_active AS IsActive
                FROM sub_category_master
                ORDER BY id DESC;
            """;

            using var conn = GetConn();
            return await conn.QueryAsync<SubCategoryMasterDto>(sql);
        }

        public async Task<SubCategoryMasterDto?> GetByIdAsync(long id)
        {
            const string sql = """
                SELECT id,
                       category_id AS CategoryId,
                       name,
                       is_active AS IsActive
                FROM sub_category_master
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QueryFirstOrDefaultAsync<SubCategoryMasterDto>(
                sql, new { Id = id });
        }

        public async Task<long> InsertAsync(SubCategoryMasterCreateDto dto)
        {
            const string sql = """
                INSERT INTO sub_category_master
                (category_id, name)
                VALUES (@CategoryId, @Name)
                RETURNING id;
            """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateAsync(long id, SubCategoryMasterUpdateDto dto)
        {
            const string sql = """
                UPDATE sub_category_master
                SET category_id = @CategoryId,
                    name = @Name,
                    is_active = @IsActive,
                    updated_at = NOW()
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                dto.CategoryId,
                dto.Name,
                dto.IsActive
            });
        }

        public async Task DeleteAsync(long id)
        {
            // Soft delete
            const string sql = """
                UPDATE sub_category_master
                SET is_active = false,
                    updated_at = NOW()
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}

