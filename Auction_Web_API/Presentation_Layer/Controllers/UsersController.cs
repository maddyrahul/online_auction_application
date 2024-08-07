using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.Data;
using Data_Access_Layer.DTOs;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        /*
                [HttpPost("LoginUser")]
                public async Task<IActionResult> Login([FromBody] LoginDto userLoginDto)
                {
                    var token = await _userService.AuthenticateAsync(userLoginDto);

                    if (token == null)
                    {
                        return Unauthorized("Invalid email or password.");
                    }

                    return Ok(new { Token = token });
                }*/


        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto userLoginDto)
        {
            try
            {
                var token = await _userService.AuthenticateAsync(userLoginDto);

                if (token == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                if (ex.Message == "User is banned.")
                {
                    return Unauthorized("User is banned.");
                }
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] UserDto userRegisterDto)
        {
            var token = await _userService.RegisterAsync(userRegisterDto);

            if (token == null)
            {
                return BadRequest("User registration failed.");
            }

            return Ok(new { Token = token });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }

            try
            {
                await _userService.UpdateUser(id, userDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
