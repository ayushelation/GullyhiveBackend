using Dapper;
using GullyHive.Admin.Models;
using Npgsql;

namespace GullyHive.Admin.Repositories
{
    public class JobStatusMasterRepository : IJobStatusMasterRepository
    {
        private readonly string _connStr;

        public JobStatusMasterRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConn() => new(_connStr);

        public async Task<IEnumerable<JobStatusMasterDto>> GetAllAsync()
        {
            const string sql = """
                SELECT id, name, is_active AS IsActive
                FROM india_leadgen.job_status_master
                ORDER BY id;
            """;

            using var conn = GetConn();
            return await conn.QueryAsync<JobStatusMasterDto>(sql);
        }

        public async Task<JobStatusMasterDto?> GetByIdAsync(long id)
        {
            const string sql = """
                SELECT id, name, is_active AS IsActive
                FROM india_leadgen.job_status_master
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QueryFirstOrDefaultAsync<JobStatusMasterDto>(sql, new { Id = id });
        }

        public async Task<long> InsertAsync(JobStatusMasterCreateDto dto)
        {
            const string sql = """
                INSERT INTO india_leadgen.job_status_master (name, is_active)
                VALUES (@Name, @IsActive)
                RETURNING id;
            """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateAsync(long id, JobStatusMasterUpdateDto dto)
        {
            const string sql = """
                UPDATE india_leadgen.job_status_master
                SET name = @Name,
                    is_active = @IsActive,
                    updated_at = NOW()
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { Id = id, dto.Name, dto.IsActive });
        }

        public async Task DeleteAsync(long id)
        {
            const string sql = """
                DELETE FROM india_leadgen.job_status_master
                WHERE id = @Id;
            """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
