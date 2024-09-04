namespace TPOS.Api.Dtos
{
    public class CustomerDto
    {
        public int CustomerID { get; set; }
        public int? UserID { get; set; }
        public int ContactID { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        // Contact Info
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
