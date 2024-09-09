namespace TPOS.Api.Dtos.Request
{
    public class BranchRequestDto: ContactInfoDto
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int CompanyID { get; set; }
        public bool Active { get; set; }
    }
}
