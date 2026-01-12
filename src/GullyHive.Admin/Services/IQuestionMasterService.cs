using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IQuestionMasterService
    {
        Task<IEnumerable<object>> GetQuestionsAsync();
        Task<long> CreateQuestionAsync(QuestionCreateDto dto);
        Task UpdateQuestionAsync(long id, QuestionUpdateDto dto);
        Task DeleteQuestionAsync(long id);

        Task<long> AddOptionAsync(long questionId, QuestionOptionDto dto);
        Task UpdateOptionAsync(long id, QuestionOptionDto dto);
        Task DeleteOptionAsync(long id);
        Task<IEnumerable<QuestionOptionDto>> GetAllOptionsAsync();
        Task<QuestionDto> GetQuestionByIdAsync(long id);
        Task<QuestionOptionDto> GetOptionByIdAsync(long id);
        Task<QuestionWithOptionsDto> GetQuestionWithOptionsAsync(long id);
    }
}
