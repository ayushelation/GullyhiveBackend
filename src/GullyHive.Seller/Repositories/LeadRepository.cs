
    using Dapper;
    using GullyHive.Seller.Models;
    using Npgsql;
namespace GullyHive.Seller.Repositories
{

    public class LeadRepository : ILeadRepository
    {
        private readonly string _connectionString;

        public LeadRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<LeadDto>> GetRecentLeadsAsync()
        {
            const string sql = @"
       SELECT
    l.id,
    u.display_name AS CustomerName,
    sc.name        AS ServiceName,
    c.name         AS Location,
    l.description AS Description,
    l.budget_min  AS BudgetMin,
    l.budget_max  AS BudgetMax,
    l.lead_status AS Status,
    l.created_at  AS CreatedAt
FROM india_leadgen.leads l
LEFT JOIN india_leadgen.users u ON u.id = l.customer_user_id
LEFT JOIN india_leadgen.service_categories sc ON sc.id = l.category_id
LEFT JOIN india_leadgen.cities c ON c.id = l.city_id
ORDER BY l.created_at DESC
LIMIT 20;
";

            using var conn = GetConnection();
            return await conn.QueryAsync<LeadDto>(sql);
        }
    }

}
