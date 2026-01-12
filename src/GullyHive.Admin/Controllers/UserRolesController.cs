using Microsoft.AspNetCore.Mvc;
using GullyHive.Admin.Models;
using GullyHive.Admin.Services;
using Microsoft.AspNetCore.Authorization;

namespace GullyHive.Admin.Controllers
{
    //    [ApiController]
    //    [Route("api/admin/user-roles")]
    //    [Authorize(Roles = "Admin")]
    //    public class UserRolesController : ControllerBase
    //    {
    //        private readonly IUserRoleService _service;

    //        public UserRolesController(IUserRoleService service)
    //        {
    //            _service = service;
    //        }

    //        [HttpGet]
    //        public async Task<IActionResult> GetAll()
    //        {
    //            var data = await _service.GetAllAsync();

    //            if (!data.Any())
    //                return Ok(new { success = false, message = "No user roles found." });

    //            return Ok(new { success = true, data });
    //        }


    //        // CREATE
    //        [HttpPost]
    //        public async Task<IActionResult> AssignRole([FromBody] UserRoleCreateDto dto)
    //        {
    //            var id = await _service.AssignRoleAsync(dto);

    //            if (id == null)
    //                return Ok(new { success = false, message = "User already has this role." });

    //            return Ok(new { success = true, id });
    //        }

    //        // READ
    //        [HttpGet("user/{userId:long}")]
    //        public async Task<IActionResult> GetRolesByUser(long userId)
    //        {
    //            return Ok(await _service.GetRolesByUserIdAsync(userId));
    //        }

    //        // UPDATE
    //        [HttpPut]
    //        public async Task<IActionResult> UpdateRole(
    //            long userId,
    //            long oldRoleId,
    //            long newRoleId)
    //        {
    //            var updated = await _service.UpdateRoleAsync(userId, oldRoleId, newRoleId);

    //            if (!updated)
    //                return Ok(new { success = false, message = "Role not found or already assigned." });

    //            return Ok(new { success = true });
    //        }

    //        // DELETE
    //        [HttpDelete]
    //        public async Task<IActionResult> RemoveRole(long userId, long roleId)
    //        {
    //            var removed = await _service.RemoveRoleAsync(userId, roleId);

    //            if (!removed)
    //                return Ok(new { success = false, message = "Role not found." });

    //            return Ok(new { success = true });
    //        }
    //    }
    //}

    //using GullyHive.Admin.Models;
    //using GullyHive.Admin.Services;
    //using Microsoft.AspNetCore.Authorization;
    //using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/admin/user-roles")]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _service;

        public UserRolesController(IUserRoleService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        // GET BY USER
        [HttpGet("user/{userId:long}")]
        public async Task<IActionResult> GetByUser(long userId)
            => Ok(await _service.GetRolesByUserIdAsync(userId));

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleCreateDto dto)
        {
            var id = await _service.AssignRoleAsync(dto);

            if (id == null)
                return Ok(new { success = false, message = "Role already assigned." });

            return Ok(new { success = true, id });
        }

        // UPDATE by ID
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UserRoleUpdateDto dto)
        {
            if (dto.UserId == null && dto.RoleId == null)
                return BadRequest("Nothing to update.");

            var result = await _service.UpdateAsync(id, dto);

            if (!result.Success)
                return Ok(new { success = false, message = result.Message });

            return Ok(new { success = true });
        }


        // DELETE by ID
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return Ok(new { success = false, message = "Record not found." });

            return Ok(new { success = true });
        }
    }
}
