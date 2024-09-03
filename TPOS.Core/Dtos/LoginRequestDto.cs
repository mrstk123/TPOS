
using System.ComponentModel.DataAnnotations;

namespace TPOS.Core.Dtos
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
