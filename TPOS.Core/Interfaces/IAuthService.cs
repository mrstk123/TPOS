using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Dtos;

namespace TPOS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponse> Login(string username, string password);
    }
}
