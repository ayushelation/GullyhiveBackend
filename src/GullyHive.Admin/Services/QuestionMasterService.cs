using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class QuestionMasterService : IQuestionMasterService
    {
        private readonly IQuestionMasterRepository _repo;

        public QuestionMasterService(IQuestionMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<object>> GetQuestionsAsync()
            => _repo.GetQuestionsAsync();
        public async Task<QuestionDto> GetQuestionByIdAsync(long id)
        {
            var question = await _repo.GetQuestionByIdAsync(id);
            if (question == null)
                throw new KeyNotFoundException("Question not found");

            return question;
        }

        public Task<long> CreateQuestionAsync(QuestionCreateDto dto)
            => _repo.InsertQuestionAsync(dto);

        public Task UpdateQuestionAsync(long id, QuestionUpdateDto dto)
            => _repo.UpdateQuestionAsync(id, dto);
        public async Task<QuestionOptionDto> GetOptionByIdAsync(long id)
        {
            var option = await _repo.GetOptionByIdAsync(id);
            if (option == null)
                throw new KeyNotFoundException("Option not found");

            return option;
        }
        public Task DeleteQuestionAsync(long id)
            => _repo.DeleteQuestionAsync(id);

        public Task<long> AddOptionAsync(long questionId, QuestionOptionDto dto)
            => _repo.InsertOptionAsync(questionId, dto);
        public Task<IEnumerable<QuestionOptionDto>> GetAllOptionsAsync()
    => _repo.GetAllOptionsAsync();
        public Task UpdateOptionAsync(long id, QuestionOptionDto dto)
            => _repo.UpdateOptionAsync(id, dto);

        public Task DeleteOptionAsync(long id)
            => _repo.DeleteOptionAsync(id);

        public async Task<QuestionWithOptionsDto> GetQuestionWithOptionsAsync(long id)
        {
            var question = await _repo.GetQuestionWithOptionsAsync(id);

            if (question == null)
                throw new KeyNotFoundException("Question not found");

            return question;
        }
    }

}
