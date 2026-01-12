using Dapper;
using Npgsql;
using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public class PartnerEarningRepository : IPartnerEarningRepository
    {
        private readonly string _connectionString;
        public PartnerEarningRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<IEnumerable<PartnerEarningDto>> GetByUserIdAsync(int userId)
        {
            using var _db = GetConnection();
            var sql = "SELECT * FROM india_leadgen.partner_earnings WHERE partner_user_id = @UserId";
            return await _db.QueryAsync<PartnerEarningDto>(sql, new { UserId = userId });
        }

        public async Task<decimal> GetTotalApprovedEarningsAsync(int userId)
        {
            using var _db = GetConnection();
            var sql = "SELECT COALESCE(SUM(amount),0) FROM india_leadgen.partner_earnings WHERE partner_user_id = @UserId AND status = 'approved'";
            return await _db.ExecuteScalarAsync<decimal>(sql, new { UserId = userId });
        }
    }
}
