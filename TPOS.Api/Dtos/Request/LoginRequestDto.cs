using System.ComponentModel.DataAnnotations;

namespace TPOS.Api.Dtos.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        // [MinLength(8)]
        public string Password { get; set; }
    }
}
