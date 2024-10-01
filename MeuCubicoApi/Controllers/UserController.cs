using DTO;
using MeuCubicoApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Shared.IServices;

namespace MeuCubicoApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService userService)
        {
            service = userService;
        }

        [HttpGet("User/{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserById(string id)
        {
            try
            {
                var dto = await service.GetUserById(id);

                if (dto == null)
                {
                    return NotFound("User not found");
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("User/pagination")]
      
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] UserParameters usersParameters)
        {
            try
            {
                var users = await service.GetAllUsers(usersParameters);

                var metadata = new
                {
                    users.TotalCount,
                    users.PageSize,
                    users.CurrentPage,
                    users.TotalPages,
                    users.HasNext,
                    users.HasPreviuos
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(users);
            }
            catch (Exception ex)
            {

                return StatusCode(404, $"Not found error: {ex.Message}");
            }
        }
    }
}
