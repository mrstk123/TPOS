
namespace TPOS.Application.Models
{
    public class UserResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
