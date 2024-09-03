
using System.ComponentModel.DataAnnotations;

namespace TPOS.Core.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
