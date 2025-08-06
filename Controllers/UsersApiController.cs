using FinanceApp.Data.Service;
using FinanceApp.Models;
using FinanceApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegistrationDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    Role = request.Role ?? "User"
                };

                await _userService.RegisterUser(user, request.Password);
                
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { message = "User created successfully", userId = user.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loginDto = new UserLoginDTO
                {
                    Username = request.Username,
                    Password = request.Password
                };
                
                var loginResponse = await _userService.LoginUser(loginDto);
                
                return Ok(new { 
                    token = loginResponse.Token, 
                    userId = loginResponse.UserId,
                    username = loginResponse.Username,
                    email = loginResponse.Email,
                    role = loginResponse.Role,
                    message = "Login successful" 
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
