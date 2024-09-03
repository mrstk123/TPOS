using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Dtos;
using TPOS.Core.Entities;

namespace TPOS.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<RegisterResponse> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<LoginResponse> LoginAsync(string username, string password);
    }
}
