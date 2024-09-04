using System.Text.Json.Serialization;
using TPOS.Core.Entities.Generated;

namespace TPOS.Api.Dtos.Response
{
    public class UserResponseDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime? Validity { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        public List<string> Roles { get; set; }
    }
}
