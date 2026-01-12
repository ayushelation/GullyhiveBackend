using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/provider-status-master")]
    [Authorize(Roles = "Admin")]
    public class ProviderStatusMasterController : ControllerBase
    {
        private readonly IProviderStatusMasterService _service;

        public ProviderStatusMasterController(
            IProviderStatusMasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ProviderStatusMasterCreateDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] ProviderStatusMasterUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { success = true, id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { success = true, id });
        }
    }
}
