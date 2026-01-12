using Dapper;
using GullyHive.Admin.Models;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class ServiceCategoryMasterRepository : IServiceCategoryMasterRepository
    {
        private readonly string _connStr;

        public ServiceCategoryMasterRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConn() => new(_connStr);

        public async Task<IEnumerable<ServiceCategoryMasterDto>> GetAllAsync()
        {
            const string sql = """
                SELECT id,
                       parent_id AS ParentId,
                       name,
                       slug,
                       description,
                       is_leaf AS IsLeaf,
                       is_active AS IsActive,
                       display_order AS DisplayOrder
                FROM india_leadgen.service_categories
                ORDER BY display_order;
            """;

            using var conn = GetConn();
            return await conn.QueryAsync<ServiceCategoryMasterDto>(sql);
        }

        public async Task<ServiceCategoryMasterDto?> GetByIdAsync(long id)
        {
            const string sql = """
                SELECT id,
                       parent_id AS ParentId,
                       name,
                       slug,
                       description,
                       is_leaf AS IsLeaf,
                       is_active AS IsActive,
                       display_order AS DisplayOrder
                FROM india_leadgen.service_categories
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QueryFirstOrDefaultAsync<ServiceCategoryMasterDto>(
                sql,
                new { Id = id }
            );
        }

        public async Task<long> InsertAsync(ServiceCategoryMasterCreateDto dto)
        {
            const string sql = """
                INSERT INTO india_leadgen.service_categories
                (parent_id, name, slug, description, is_leaf, display_order)
                VALUES (@ParentId, @Name, @Slug, @Description, @IsLeaf, @DisplayOrder)
                RETURNING id;
            """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateAsync(long id, ServiceCategoryMasterUpdateDto dto)
        {
            const string sql = """
                UPDATE india_leadgen.service_categories
                SET parent_id = @ParentId,
                    name = @Name,
                    slug = @Slug,
                    description = @Description,
                    is_leaf = @IsLeaf,
                    is_active = @IsActive,
                    display_order = @DisplayOrder,
                    updated_at = NOW()
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                dto.ParentId,
                dto.Name,
                dto.Slug,
                dto.Description,
                dto.IsLeaf,
                dto.IsActive,
                dto.DisplayOrder
            });
        }

        public async Task DeleteAsync(long id)
        {
            const string sql =
                "DELETE FROM india_leadgen.service_categories WHERE id = @Id;";

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
