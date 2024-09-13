using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPOS.Application.Interfaces;
using TPOS.Application.Interfaces.Services;
using TPOS.Application.Models;
using TPOS.Domain.Entities.Generated;

namespace TPOS.Infrastructure.Security
{
    public class AuthService : IAuthService
    {
        // private readonly AppDbContext _context;  // Directly using DbContext provided by EF Core
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(// AppDbContext context,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            // _context = context;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAsync(
                 filter: user => user.Active,
                 include: user => user.Include(u => u.UserRoles.Where(x => x.Active)).ThenInclude(ur => ur.Role));
            return users;
        }

        public async Task<RegisterResponse> RegisterAsync(string userName, string password, string? email)
        {
            // Check if the user already exists
            // var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var existingUser = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.UserName == userName);
            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Username already exists."
                };
            }

            // Hash the password
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new user
            var newUser = new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = email,
                // Validity = DateTime.UtcNow.AddMinutes(30)
            };

            //await _context.Users.AddAsync(newUser);
            //await _context.SaveChangesAsync();

            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();

            // var userRoleId = _context.Roles.FirstOrDefault(x => x.RoleName == "User")?.RoleID ?? 0;
            var userRoleId = (await _unitOfWork.RoleRepository.FindAsync(x => x.RoleName == "User")).FirstOrDefault()?.RoleID ?? 0;
            var newUserRole = new UserRole { UserID = newUser.UserID, RoleID = userRoleId };

            //await _context.UserRoles.AddAsync(newUserRole);
            //await _context.SaveChangesAsync();

            await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);
            await _unitOfWork.CompleteAsync();

            return new RegisterResponse
            {
                Success = true,
                Message = "Registration successful."
            };
        }

        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            //var user = await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(u => u.UserName == userName && x.Active);
            var user = await _unitOfWork.UserRepository.GetSingleAsync(
                 filter: user => user.UserName == userName && user.Active,
                 include: user => user.Include(u => u.UserRoles.Where(x => x.Active)).ThenInclude(ur => ur.Role));

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

            var token = await CreateTokenAsync(user);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                User = new UserResponse
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = user.UserRoles.Select(x => x.Role.RoleName).Distinct().ToList()
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

        private async Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                                new Claim(ClaimTypes.Name, user.UserName)
                            };

            // var userRoles = _context.UserRoles.Where(ur => ur.UserID == user.UserID).Include(ur => ur.Role).ToList();
            var userRoles = (await _unitOfWork.UserRoleRepository.GetAsync(
                filter: ur => ur.UserID == user.UserID,
                include: x => x.Include(ur => ur.Role))).ToList();
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