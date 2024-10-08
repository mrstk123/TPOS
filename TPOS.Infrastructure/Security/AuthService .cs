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
        private readonly ITokenService _tokenService;

        public AuthService(// AppDbContext context,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            // _context = context;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        // Add GetUsersAsync() here cuz I don't want to use _unitOfWork in AccountController and also don't wanna create new service
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
                Validity = DateTime.UtcNow.AddMinutes(30)
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

        public async Task<LoginResponse> LoginAsync(string loginIdentifier, string password)
        {
            //var user = await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(u => u.UserName == userName && x.Active);
            var user = await _unitOfWork.UserRepository.GetSingleAsync(
                 filter: user => (user.UserName.ToLower() == loginIdentifier.ToLower() || user.Email.ToLower() == loginIdentifier.ToLower()) && user.Active,
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

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
            var refreshTokenExpiry = _configuration.GetValue<int>("Jwt:RefreshTokenExpiryInMinutes"); // using Microsoft.Extensions.Configuration.Binder package
            //int.TryParse(_configuration["Jwt:RefreshTokenExpiryInMinutes"], out int refreshTokenExpiry); // without using package

            // Update refreshToken of the user 
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenExpiry);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return new LoginResponse
            {
                Success = true,
                Token = accessToken,
                User = new UserResponse
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = user.UserRoles.Select(x => x.Role.RoleName).Distinct().ToList()
                }
            };
        }

        public async Task<LogoutResponse> LogoutAsync(int userID)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userID);
            if (user == null)
            {
               return new LogoutResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            await ClearRefreshToken(user);
            return new LogoutResponse
            {
                Success = true,
                Message = "Logged out successfully."
            };
        }

        public async Task<bool> RevokeAsync(int userID)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userID);
            if (user == null)
            {
                return false; // User not found
            }

            await ClearRefreshToken(user);
            return true;
        }

        public async Task<Token> RefreshAsync(Token token)
        {
            // Validate Expired Access Token (without checking expiration date) and then Validate Refresh Token
            // Get userId from the Expired Access Token
            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);  // Don't check the token's expiration date
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);  
            //var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub); // even if user id is stored in sub, can only access with NameIdentifier becasue OIDC StandardClaims are renamed on JWT token handler 
            if (userIdClaim == null)
            {
                // return Unauthorized("Invalid access token claims.");
                throw new SecurityTokenException("Invalid attempt.");
            }

            var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(Convert.ToInt16(userIdClaim.Value));
            // Validate the Refresh Token
            if (existingUser == null || existingUser.RefreshToken != token.RefreshToken || DateTime.UtcNow > existingUser.RefreshTokenExpiryTime)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }

            var newAccessToken = await _tokenService.GenerateAccessTokenAsync(existingUser);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
            var refreshTokenExpiry = _configuration.GetValue<int>("Jwt:RefreshTokenExpiryInMinutes"); // using Microsoft.Extensions.Configuration.Binder package
            //int.TryParse(_configuration["Jwt:RefreshTokenExpiryInMinutes"], out int refreshTokenExpiry); // without using package

            existingUser.RefreshToken = newRefreshToken;
            existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenExpiry);
            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.CompleteAsync();

            return new Token
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
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

        private async Task ClearRefreshToken(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}