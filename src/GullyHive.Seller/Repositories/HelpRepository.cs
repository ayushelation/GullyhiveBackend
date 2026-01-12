using Dapper;
using GullyHive.Seller.Models;
using Npgsql;

namespace GullyHive.Seller.Repositories
{
    public class HelpRepository : IHelpRepository
    {
        private readonly string _connectionString;

        public HelpRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<(List<HelpCategoryDto>, List<HelpFaqDto>)> GetHelpDataAsync()
        {
            const string sql = @"
        SELECT 
            c.id,
            c.title,
            c.description,
            c.icon,
            COUNT(f.id) AS articles
        FROM india_leadgen.help_categories c
        LEFT JOIN india_leadgen.help_faqs f
            ON f.category_id = c.id
            AND f.is_active = true
        WHERE c.is_active = true
        GROUP BY c.id
        ORDER BY c.display_order;

        SELECT 
            id,
            question,
            answer
        FROM india_leadgen.help_faqs
        WHERE is_active = true
        ORDER BY display_order;
    ";

            using var conn = GetConnection();
            using var multi = await conn.QueryMultipleAsync(sql);

            var categories = (await multi.ReadAsync<HelpCategoryDto>()).ToList();
            var faqs = (await multi.ReadAsync<HelpFaqDto>()).ToList();

            return (categories, faqs);
        }


    }
}
