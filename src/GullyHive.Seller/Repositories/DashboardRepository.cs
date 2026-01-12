using Dapper;
using GullyHive.Seller.Models;
using Npgsql;

namespace GullyHive.Seller.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<SellerDto?> GetSellerByUsernameAsync(string username)
        {
            const string sql = @"
             SELECT id, email, display_name AS DisplayName
             FROM users
             WHERE lower(display_name) = lower(@Username)  
             OR lower(email) = lower(@Username)        
             OR phone = @Username                      
             LIMIT 1";


            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<SellerDto>(sql, new { Username = username });
        }

        public async Task<SellerStatsDto> GetSellerStatsAsync(long sellerId)
        {
            const string sql = @"
            SELECT 
                (SELECT COUNT(*) FROM leads WHERE customer_user_id = @SellerId) AS TotalLeads";

            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<SellerStatsDto>(sql, new { SellerId = sellerId });
        }

        public async Task<IEnumerable<LeadDto>> GetRecentLeadsAsync(long sellerId)
        {
            const string sql = @"
            SELECT id, lead_type AS CustomerName, description AS ServiceName, created_at AS CreatedAt
            FROM leads
            WHERE customer_user_id = @SellerId
            ORDER BY created_at DESC
            LIMIT 5";

            using var conn = GetConnection();
            return await conn.QueryAsync<LeadDto>(sql, new { SellerId = sellerId });
        }
    }
}
