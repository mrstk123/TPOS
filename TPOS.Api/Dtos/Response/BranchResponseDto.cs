namespace TPOS.Api.Dtos.Response
{
    public class BranchResponseDto: ContactInfoDto
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int CompanyID { get; set; }

        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime UpdatedOn { get; set; }
        //public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        // Company
        public string CompanyName = null!;
        public string? CompanyAddress { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyEmail { get; set; }
    }
}
