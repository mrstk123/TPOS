using Microsoft.AspNetCore.Mvc;
using TPOS.Core.Dtos;
using TPOS.Core.Interfaces;

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            registerRequestDto.UserName = registerRequestDto.UserName.ToLower();
            var response = await _authService.Register(registerRequestDto);

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
            var result = await _authService.Login(userLoginDto.UserName.ToLower(), userLoginDto.Password);

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