using Domain.Request;
using Microsoft.AspNetCore.Mvc;
using Service;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var users = _userService.List();
            return Ok(users);
        }

        [HttpPost("Register")]
        [AllowAnonymous] // Acesso anônimo
        public IActionResult Post([FromBody] UserRequest user)
        {
            var newUser = _userService.Create(user);
            return Ok(newUser);
        }

        [HttpPut("Update/{userId}")]
        public IActionResult Put([FromBody] UpdatedUserRequest user, int userId)
        {
            user.Id = userId;
            var updatedUser = _userService.Update(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{idUsuario}")]
        public IActionResult Delete(int idUsuario)
        {
            _userService.Delete(idUsuario);
            return NoContent();
        }
    }
}
