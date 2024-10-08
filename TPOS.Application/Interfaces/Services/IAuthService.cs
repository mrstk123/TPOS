using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Application.Models;
using TPOS.Domain.Entities.Generated;

namespace TPOS.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<RegisterResponse> RegisterAsync(string userName, string password, string? email);
        Task<LoginResponse> LoginAsync(string loginIdentifier, string password);    // user can login with userName or Email
        Task<LogoutResponse> LogoutAsync(int useID);
        Task<bool> RevokeAsync(int userID);
        Task<Token> RefreshAsync(Token token);
    }
}
