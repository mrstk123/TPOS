namespace TPOS.Api.Dtos.Response
{
    public class CustomerResponseDto: ContactInfoDto
    {
        public int CustomerID { get; set; }
        public int? UserID { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }

    }
}
