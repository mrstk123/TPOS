namespace TPOS.Api.Dtos.Request
{
    public class CustomerRequestDto: ContactInfoDto
    {
        public int CustomerID { get; set; }
        public int? UserID { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool Active { get; set; }

    }
}
