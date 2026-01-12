using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/states")]
    [Authorize(Roles = "Admin")]
    public class StateMasterController : ControllerBase
    {
        private readonly IStateMasterService _service;

        public StateMasterController(IStateMasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetStates() => Ok(await _service.GetStatesAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetStateById(long id) => Ok(await _service.GetStateByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateCreateDto dto)
        {
            var id = await _service.CreateStateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateState(long id, [FromBody] StateUpdateDto dto)
        {
            await _service.UpdateStateAsync(id, dto);
            return Ok(new
            {
                success = true,
                stateId = id
            });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteState(long id)
        {
            await _service.DeleteStateAsync(id);
            return Ok(new
            {
                success = true,
                stateId = id
            });
        }

    }
}
