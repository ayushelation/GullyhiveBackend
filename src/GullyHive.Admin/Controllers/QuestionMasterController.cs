using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionMasterController : ControllerBase
    {
        private readonly IQuestionMasterService _service;

        public QuestionMasterController(IQuestionMasterService service)
        {
            _service = service;
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { res = "Admin module alive" });
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            return Ok(await _service.GetQuestionsAsync());
        }
        [HttpGet("questions/{id:long}")]
        public async Task<IActionResult> GetQuestionById(long id)
        {
            var question = await _service.GetQuestionByIdAsync(id);
            return Ok(question);
        }

        [HttpPost("questions")]
        public async Task<IActionResult> CreateQuestion(
            [FromBody] QuestionCreateDto dto)
        {
            var id = await _service.CreateQuestionAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("questions/{id:long}")]
        public async Task<IActionResult> UpdateQuestion(
            long id, [FromBody] QuestionUpdateDto dto)
        {
            await _service.UpdateQuestionAsync(id, dto);
            return Ok(new
            {
                success = true,
                message = "Question updated successfully",
                questionId = id
            });
        }

        [HttpDelete("questions/{id:long}")]
        public async Task<IActionResult> DeleteQuestion(long id)
        {
            await _service.DeleteQuestionAsync(id);
            return Ok(new
            {
                success = true,
                message = "Question deleted successfully",
                questionId = id
            });
        }

        // ---------------- OPTIONS ----------------

        [HttpPost("questions/{questionId:long}/options")]
        public async Task<IActionResult> AddOption(
            long questionId, [FromBody] QuestionOptionDto dto)
        {
            var id = await _service.AddOptionAsync(questionId, dto);
            return Ok(new { id });
        }

        [HttpGet("options/{id:long}")]
        public async Task<IActionResult> GetOptionById(long id)
        {
            var option = await _service.GetOptionByIdAsync(id);
            return Ok(option);
        }


        [HttpPut("options/{id:long}")]
        public async Task<IActionResult> UpdateOption(
            long id, [FromBody] QuestionOptionDto dto)
        {
            await _service.UpdateOptionAsync(id, dto);
            return Ok(new { message = "Option updated successfully" });
        }
        [HttpGet("options")]
        public async Task<IActionResult> GetAllOptions()
        {
            var options = await _service.GetAllOptionsAsync();
            return Ok(options);
        }

        [HttpDelete("options/{id:long}")]
        public async Task<IActionResult> DeleteOption(long id)
        {
            await _service.DeleteOptionAsync(id);
            return Ok(new { message = "Option deleted successfully" });
        }

        [HttpGet("questions/{id:long}/with-options")]
        public async Task<IActionResult> GetQuestionWithOptions(long id)
        {
            var result = await _service.GetQuestionWithOptionsAsync(id);
            return Ok(result);
        }
    }
}
