using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using Npgsql;
using Dapper;

namespace GullyHive.Admin.Repositories
{
    public class QuestionMasterRepository : IQuestionMasterRepository
    {
        private readonly string _connStr;

        public QuestionMasterRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConn() => new(_connStr);

        public async Task<IEnumerable<object>> GetQuestionsAsync()
        {
            const string sql = """
        SELECT q.*, 
               COALESCE(json_agg(o.*) 
               FILTER (WHERE o.id IS NOT NULL), '[]') AS options
        FROM india_leadgen.requirement_questions q
        LEFT JOIN india_leadgen.requirement_question_options o
            ON o.question_id = q.id
        GROUP BY q.id
        ORDER BY q.display_order;
        """;

            using var conn = GetConn();
            return await conn.QueryAsync(sql);
        }

        public async Task<long> InsertQuestionAsync(QuestionCreateDto dto)
        {
            const string sql = """
        INSERT INTO india_leadgen.requirement_questions
        (category_id, question_text, question_type, is_mandatory, display_order)
        VALUES (@CategoryId, @QuestionText, @QuestionType, @IsMandatory, @DisplayOrder)
        RETURNING id;
        """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task UpdateQuestionAsync(long id, QuestionUpdateDto dto)
        {
            const string sql = """
            UPDATE india_leadgen.requirement_questions
            SET question_text = @QuestionText,
                is_mandatory  = @IsMandatory,
                display_order = @DisplayOrder,
                is_active     = @IsActive,
                updated_at    = NOW()
            WHERE id = @Id;
            """;

            using var conn = GetConn();

            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                dto.QuestionText,
                dto.IsMandatory,
                dto.DisplayOrder,
                dto.IsActive
            });
        }


        public async Task DeleteQuestionAsync(long id)
        {
            using var conn = GetConn();
            await conn.ExecuteAsync(
                "DELETE FROM india_leadgen.requirement_questions WHERE id = @id",
                new { id });
        }
        public async Task<QuestionDto?> GetQuestionByIdAsync(long id)
        {
            const string sql = """
            SELECT
                q.id,
                q.category_id       AS CategoryId,
                q.question_text     AS QuestionText,
                q.question_type     AS QuestionType,
                q.is_mandatory      AS IsMandatory,
                q.display_order     AS DisplayOrder,
                q.is_active         AS IsActive,
                q.created_at        AS CreatedAt,
                q.updated_at        AS UpdatedAt
            FROM india_leadgen.requirement_questions q
            WHERE q.id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QuerySingleOrDefaultAsync<QuestionDto>(sql, new { Id = id });
        }


        public async Task<long> InsertOptionAsync(long questionId, QuestionOptionDto dto)
        {
            const string sql = """
            INSERT INTO india_leadgen.requirement_question_options
            (question_id, option_text, display_order, is_active)
            SELECT
                @QuestionId,
                @OptionText,
                COALESCE(MAX(display_order), 0) + 1,
                @IsActive
            FROM india_leadgen.requirement_question_options
            WHERE question_id = @QuestionId
            RETURNING id;
            """;

            using var conn = GetConn();
            return await conn.ExecuteScalarAsync<long>(sql, new
            {
                QuestionId = questionId,
                OptionText = dto.OptionText,
                IsActive = dto.IsActive
            });
        }

        public async Task<QuestionOptionDto?> GetOptionByIdAsync(long id)
        {
            const string sql = """
            SELECT
                id,
                question_id     AS QuestionId,
                option_text     AS OptionText,
                display_order   AS DisplayOrder,
                is_active       AS IsActive
            FROM india_leadgen.requirement_question_options
            WHERE id = @Id;
            """;

            using var conn = GetConn();
            return await conn.QuerySingleOrDefaultAsync<QuestionOptionDto>(sql, new { Id = id });
        }


        public async Task UpdateOptionAsync(long id, QuestionOptionDto dto)
        {
            const string sql = """
        UPDATE india_leadgen.requirement_question_options
        SET option_text = @OptionText,
            display_order = @DisplayOrder,
            is_active = @IsActive
        WHERE id = @Id;
        """;

            using var conn = GetConn();
            await conn.ExecuteAsync(sql, new { dto.OptionText, dto.DisplayOrder, dto.IsActive, Id = id });
        }

        public async Task DeleteOptionAsync(long id)
        {
            using var conn = GetConn();
            await conn.ExecuteAsync(
                "DELETE FROM india_leadgen.requirement_question_options WHERE id = @id",
                new { id });
        }

        public async Task<IEnumerable<QuestionOptionDto>> GetAllOptionsAsync()
        {
            const string sql = """
            SELECT
                id,
                question_id AS QuestionId,
                option_text AS OptionText,
                display_order AS DisplayOrder,
                is_active AS IsActive
            FROM india_leadgen.requirement_question_options
            ORDER BY question_id, display_order;
            """;

            using var conn = GetConn();
            return await conn.QueryAsync<QuestionOptionDto>(sql);
        }


        // Get Questions with options
        public async Task<QuestionWithOptionsDto?> GetQuestionWithOptionsAsync(long id)
        {
            const string sql = """
            SELECT
                q.id,
                q.category_id       AS CategoryId,
                q.question_text     AS QuestionText,
                q.question_type     AS QuestionType,
                q.is_mandatory      AS IsMandatory,
                q.display_order     AS DisplayOrder,
                q.is_active         AS IsActive,

                o.id                AS Id,
                o.question_id       AS QuestionId,
                o.option_text       AS OptionText,
                o.display_order     AS DisplayOrder,
                o.is_active         AS IsActive
            FROM india_leadgen.requirement_questions q
            LEFT JOIN india_leadgen.requirement_question_options o
                   ON o.question_id = q.id
            WHERE q.id = @Id
            ORDER BY o.display_order;
            """;
            using var conn = GetConn();

            QuestionWithOptionsDto? question = null;

            await conn.QueryAsync<QuestionWithOptionsDto, QuestionOptionDto, QuestionWithOptionsDto>(
                sql,
                (q, o) =>
                {
                    if (question == null)
                    {
                        question = q;
                        question.Options = new List<QuestionOptionDto>();
                    }

                    if (o != null && o.Id != 0)
                        question.Options.Add(o);

                    return question;
                },
                new { Id = id },
                splitOn: "Id"
            );

            return question;
        }
    }

}
