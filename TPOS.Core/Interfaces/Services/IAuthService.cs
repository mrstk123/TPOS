using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Models;

namespace TPOS.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<RegisterResponse> RegisterAsync(string userName, string password, string? email);
        Task<LoginResponse> LoginAsync(string username, string password);
    }
}
