﻿
namespace TPOS.Application.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public UserResponse User { get; set; }
    }
}
