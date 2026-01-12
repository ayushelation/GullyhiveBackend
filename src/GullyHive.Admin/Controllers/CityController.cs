using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GullyHive.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/cities")]
    [Authorize(Roles = "Admin")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _service;

        public CityController(ICityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities() => Ok(await _service.GetAllCitiesAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCity(long id)
        {
            var city = await _service.GetCityByIdAsync(id);
            if (city == null) return NotFound(new { success = false, message = $"City with id {id} not found" });

            return Ok(new { success = true, data = city });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityCreateDto dto)
        {
            var id = await _service.CreateCityAsync(dto);
            return Ok(new { success = true, message = "City created", cityId = id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCity(long id, [FromBody] CityUpdateDto dto)
        {
            var result = await _service.UpdateCityAsync(id, dto);
            if (!result.Success) return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCity(long id)
        {
            var result = await _service.DeleteCityAsync(id);
            if (!result.Success) return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }
    }
}
