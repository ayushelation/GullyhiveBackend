using Dapper;
using Npgsql;
using GullyHive.Seller.Models;

namespace GullyHive.Seller.Repositories
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly string _connectionString;
        public ReferralRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

   
        public async Task<IEnumerable<ReferralDto>> GetByUserIdAsync(int userId)
        {
            using var db = GetConnection();

            var sql = @"
    SELECT
        r.id AS Id,
        r.code AS Code,
        r.referrer_user_id AS ReferrerUserId,
        r.referrer_role AS ReferrerRole,
        r.referred_type AS ReferredType,
        r.referred_user_id AS ReferredUserId,
        r.source AS Source,
        r.created_at AS CreatedAt,

        COALESCE(u.display_name, 'User ' || u.id) AS Name,
        u.display_name AS Avatar,
        u.created_at AS JoinedDate,
        COALESCE(SUM(pe.amount), 0) AS Earnings,
        CASE
            WHEN COUNT(pe.id) = 0 THEN 'pending'
            WHEN BOOL_AND(pe.status='paid') THEN 'paid'
            WHEN BOOL_AND(pe.status='approved') THEN 'approved'
            ELSE 'pending'
        END AS Status

    FROM referrals r
    LEFT JOIN users u ON u.id = r.referred_user_id
    LEFT JOIN partner_earnings pe 
        ON pe.referral_id = r.id
       AND pe.status IN ('pending','approved','paid')
    WHERE r.referrer_user_id = @UserId
    GROUP BY r.id, u.id
    ORDER BY r.created_at DESC;
    ";

            return await db.QueryAsync<ReferralDto>(sql, new { UserId = userId });
        }


    }
}
