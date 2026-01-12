using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/system-settings")]
    [Authorize(Roles = "Admin")]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingRepository _service;

        public SystemSettingsController(ISystemSettingRepository service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET BY KEY
        [HttpGet("{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var setting = await _service.GetByKeyAsync(key);
            if (setting == null)
                return NotFound(new { success = false, message = "Setting not found." });

            return Ok(new { success = true, data = setting });
        }

        // CREATE or UPDATE (UPSERT)
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] SystemSettingCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpsertAsync(dto);
            return Ok(new { success = true, message = "Setting saved successfully." });
        }

        // DELETE
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            var result = await _service.DeleteAsync(key);
            if (!result)
                return NotFound(new { success = false, message = "Setting not found." });

            return Ok(new { success = true, message = "Setting deleted." });
        }
    }
}
