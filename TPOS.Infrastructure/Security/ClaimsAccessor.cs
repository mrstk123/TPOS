using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TPOS.Infrastructure.Security
{
    public static class ClaimsAccessor
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static ClaimsPrincipal? CurrentUser => _httpContextAccessor?.HttpContext?.User;

        public static string UserName => GetUserName();

        public static int UserID => GetUserID();


        private static string GetUserName()
        {
            return CurrentUser?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        private static int GetUserID()
        {
            var userID = CurrentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userID ?? "0");
        }

    }
}
