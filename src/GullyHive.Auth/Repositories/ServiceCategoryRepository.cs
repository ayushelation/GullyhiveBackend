using Dapper;
using GullyHive.Auth.Models;
using GullyHive.Auth.Repositories;
using Npgsql;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly string _connectionString;

    public ServiceCategoryRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("ConStr")!;
    }

    private NpgsqlConnection GetConnection()
        => new(_connectionString);

    // 🔹 Parent Services
    public async Task<IEnumerable<ServiceCategoryDto>> GetParentCategoriesAsync()
    {
        const string sql = @"
            SELECT id, name
            FROM india_leadgen.service_categories
            WHERE is_active = true
            ORDER BY display_order, name;
        ";

        using var conn = GetConnection();
        return await conn.QueryAsync<ServiceCategoryDto>(sql);
    }

    // 🔹 Sub Categories
    public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(long parentId)
    {
        const string sql = @"
            SELECT id, name
            FROM india_leadgen.sub_category_master
            WHERE category_id = @ParentId
              AND is_active = true
            ORDER BY name;
        ";

        using var conn = GetConnection();
        return await conn.QueryAsync<SubCategoryDto>(sql, new { ParentId = parentId });
    }
}
