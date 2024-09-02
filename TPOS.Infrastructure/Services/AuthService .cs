using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPOS.Core.Dtos;
using TPOS.Core.Interfaces;
using TPOS.Core.Entities;

namespace TPOS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<RegisterResponse> Register(RegisterRequestDto registerRequestDto)
        {
            // Check if the user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == registerRequestDto.UserName);
            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Username already exists."
                };
            }

            // Hash the password
            CreatePasswordHash(registerRequestDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new user
            var newUser = new User
            {
                UserName = registerRequestDto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = registerRequestDto.Email,
                // Validity = DateTime.UtcNow.AddMinutes(30)
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var userRoleId = _context.Roles.FirstOrDefault(x => x.RoleName == "User")?.RoleID ?? 0;
            var newUserRole = new UserRole { UserID = newUser.UserID, RoleID = userRoleId };

            await _context.UserRoles.AddAsync(newUserRole);
            await _context.SaveChangesAsync();

            return new RegisterResponse
            {
                Success = true,
                Message = "Registration successful."
            };
        }

        public async Task<LoginResponse> Login(string userName, string password)
        {
            var user = await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            // Check if the validity date has passed
            if (user.Validity != null && user.Validity < DateTime.UtcNow) // Assuming UTC time for consistency
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Account is not valid or has expired."
                };
            }

            var token = CreateToken(user);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                User = new UserDto
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = user.UserRoles.Select(x => x.Role.RoleName).ToList<string>()
                }
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                                new Claim(ClaimTypes.Name, user.UserName)
                            };

            var userRoles = _context.UserRoles.Where(ur => ur.UserID == user.UserID).Include(ur => ur.Role).ToList();
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"] ?? string.Empty)),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}