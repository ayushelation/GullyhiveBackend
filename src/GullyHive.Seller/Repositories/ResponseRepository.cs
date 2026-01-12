using Dapper;
using GullyHive.Seller.Models;
using Microsoft.AspNetCore.Connections;
using Npgsql;

namespace GullyHive.Seller.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly string _connectionString;

        public ResponseRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }
        private NpgsqlConnection GetConnection() => new(_connectionString);
        public async Task<IEnumerable<ResponseDto>> GetByUserAsync(long userId)
        {
            const string sql = @"
                SELECT
                    lr.id,
                    lr.lead_id AS LeadId,
                    u.display_name AS LeadName,
                    sc.name AS Service,
                    lr.quote_amount AS QuoteAmount,
                    lr.message,
                    lr.status,
                    lr.created_at AS CreatedAt
                FROM india_leadgen.lead_responses lr
                JOIN india_leadgen.leads l ON l.id = lr.lead_id
                JOIN india_leadgen.users u ON u.id = l.customer_user_id
                JOIN india_leadgen.service_categories sc ON sc.id = l.category_id
                WHERE lr.user_id = @UserId
                ORDER BY lr.created_at DESC";

            using var conn = GetConnection();
            return await conn.QueryAsync<ResponseDto>(sql, new { UserId = userId });
        }

        public async Task<ResponseDto?> GetByIdAsync(long id, long userId)
        {
            const string sql = @"
                SELECT
                    lr.id,
                    lr.lead_id AS LeadId,
                    u.display_name AS LeadName,
                    sc.name AS Service,
                    lr.quote_amount AS QuoteAmount,
                    lr.message,
                    lr.status,
                    lr.created_at AS CreatedAt
                FROM india_leadgen.lead_responses lr
                JOIN india_leadgen.leads l ON l.id = lr.lead_id
                JOIN india_leadgen.users u ON u.id = l.customer_user_id
                JOIN india_leadgen.service_categories sc ON sc.id = l.category_id
                WHERE lr.id = @Id AND lr.user_id = @UserId";

            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<ResponseDto>(sql, new { Id = id, UserId = userId });
        }

        public async Task<long> CreateAsync(long userId, CreateResponseDto dto)
        {
            const string sql = @"
                INSERT INTO india_leadgen.lead_responses
                (lead_id, user_id, quote_amount, message, status)
                VALUES (@LeadId, @UserId, @QuoteAmount, @Message, 'pending')
                RETURNING id";

            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<long>(sql, new
            {
                dto.LeadId,
                UserId = userId,
                dto.QuoteAmount,
                dto.Message
            });
        }

        public async Task<bool> UpdateAsync(long id, long userId, UpdateResponseDto dto)
        {
            const string sql = @"
                UPDATE india_leadgen.lead_responses
                SET quote_amount = @QuoteAmount,
                    message = @Message,
                    updated_at = NOW()
                WHERE id = @Id AND user_id = @UserId";

            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, new
            {
                Id = id,
                UserId = userId,
                dto.QuoteAmount,
                dto.Message
            }) > 0;
        }

        public async Task<bool> UpdateStatusAsync(long id, long userId, string status)
        {
            const string sql = @"
                UPDATE india_leadgen.lead_responses
                SET status = @Status,
                    updated_at = NOW()
                WHERE id = @Id AND user_id = @UserId";

            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, new { Id = id, UserId = userId, Status = status }) > 0;
        }

        public async Task<bool> DeleteAsync(long id, long userId)
        {
            const string sql = @"
                DELETE FROM india_leadgen.lead_responses
                WHERE id = @Id AND user_id = @UserId";

            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, new { Id = id, UserId = userId }) > 0;
        }
    }

}
