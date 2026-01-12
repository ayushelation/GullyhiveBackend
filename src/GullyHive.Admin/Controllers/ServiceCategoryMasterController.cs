using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/service-category-master")]
    [Authorize(Roles = "Admin")]
    public class ServiceCategoryMasterController : ControllerBase
    {
        private readonly IServiceCategoryMasterService _service;

        public ServiceCategoryMasterController(IServiceCategoryMasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ServiceCategoryMasterCreateDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] ServiceCategoryMasterUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new
            {
                success = true,
                categoryId = id
            });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok(new
            {
                success = true,
                categoryId = id
            });
        }
    }
}
