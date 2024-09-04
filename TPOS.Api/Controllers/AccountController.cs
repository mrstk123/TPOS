using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPOS.Api.Dtos.Request;
using TPOS.Api.Dtos.Response;
using TPOS.Core.Interfaces.Services;

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return Ok(userDtos);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            //if (!ModelState.IsValid) // Comment because ActionFilter is defined globally and used at Controller level
            //{
            //    return BadRequest(ModelState);
            //}

            registerRequestDto.UserName = registerRequestDto.UserName.ToLower();
            var response = await _authService.RegisterAsync(registerRequestDto.UserName, registerRequestDto.Password, registerRequestDto.Email);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto userLoginDto)
        {
            var result = await _authService.LoginAsync(userLoginDto.UserName.ToLower(), userLoginDto.Password);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                token = result.Token,
                user = result.User
            });
        }
    }
}