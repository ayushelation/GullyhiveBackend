using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RoleUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { success = true, roleId = id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { success = true, roleId = id });
        }

        [HttpGet("check-table/{tableName}")]
        public async Task<IActionResult> CheckTable(string tableName)
        {
            var exists = await _service.CheckTableAsync(tableName);
            return Ok(new { TableName = tableName, Exists = exists });
        }
    }
}
