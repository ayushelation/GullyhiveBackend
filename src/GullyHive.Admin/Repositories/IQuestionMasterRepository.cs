using GullyHive.Admin.Models;

namespace GullyHive.Admin.Repositories
{
    public interface IQuestionMasterRepository
    {
        Task<IEnumerable<object>> GetQuestionsAsync();
        Task<long> InsertQuestionAsync(QuestionCreateDto dto);
        Task UpdateQuestionAsync(long id, QuestionUpdateDto dto);
        Task DeleteQuestionAsync(long id);

        Task<long> InsertOptionAsync(long questionId, QuestionOptionDto dto);
        Task UpdateOptionAsync(long id, QuestionOptionDto dto);
        Task<IEnumerable<QuestionOptionDto>> GetAllOptionsAsync();
        Task DeleteOptionAsync(long id);
        Task<QuestionDto?> GetQuestionByIdAsync(long id);
        Task<QuestionOptionDto?> GetOptionByIdAsync(long id);
        Task<QuestionWithOptionsDto?> GetQuestionWithOptionsAsync(long id);
    }
}
