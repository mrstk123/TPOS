using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TPOS.Application.Interfaces;
using TPOS.Application.Interfaces.Services;
using TPOS.Domain.Entities.Generated;

namespace TPOS.Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var claims = new List<Claim>
                            {
                                //new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()), // Sub claim in the JWT is a standard claim that is typically used to store the unique identifier for the subject (in this case, the user id)
                                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // NameIdentifier is predefined claim type used in .NET to represent the user's unique identifier
                                new Claim(ClaimTypes.Name, user.UserName)
                            };

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
                //Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:AccessTokenExpiryInMinutes"] ?? string.Empty)),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("Jwt:AccessTokenExpiryInMinutes")), // using Microsoft.Extensions.Configuration.Binder package
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // Ensure refresh token is unique in the db
            var isMultiple = (await _unitOfWork.UserRepository.FindAsync(x => x.RefreshToken == token)).Any();
            if (isMultiple) return await GenerateRefreshTokenAsync();
            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateActor = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateLifetime = false // we don't care about the token's expiration date
            };

            // Token Integrity (ensure access token has not been tampered with)
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out SecurityToken validatedToken);
            var jwtSecurityToken = validatedToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
