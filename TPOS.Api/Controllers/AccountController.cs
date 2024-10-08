using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TPOS.Api.Dtos.Request;
using TPOS.Api.Dtos.Response;
using TPOS.Application.Interfaces.Services;
using TPOS.Application.Models;

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

            try
            {
                registerRequestDto.UserName = registerRequestDto.UserName.ToLower();
                var result = await _authService.RegisterAsync(registerRequestDto.UserName, registerRequestDto.Password, registerRequestDto.Email);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto userLoginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(userLoginDto.LoginIdentifier, userLoginDto.Password);

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
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userID = GetUserIdFromToken();
                if (userID == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var result = await _authService.LogoutAsync(userID.Value);

                if (!result.Success)
                {
                    return Unauthorized(new { message = result.Message });
                }

                return Ok(new { message = "Successfully logged out." });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            try
            {
                var userID = GetUserIdFromToken();
                if (userID == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var result = await _authService.RevokeAsync(userID.Value);
                if (!result)
                {
                    return BadRequest(new { message = "Failed to revoke token." });
                }

                return Ok(new { message = "Token revoked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(Token token)
        {
            try
            {
                var response = await _authService.RefreshAsync(token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        private int? GetUserIdFromToken()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();

                try
                {
                    // Validate the token (make sure your token validation parameters are correct)
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken != null)
                    {
                        //var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals("nameid")); 
                        //var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sub)); 
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                        {
                            return userId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Token validation error: {ex.Message}");
                }
            }

            return null; // Return null if no user ID found or token is invalid
        }

    }
}