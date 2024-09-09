namespace TPOS.Api.Dtos.Response
{
    public class EmployeeResponseDto: ContactInfoDto
    {
        public int EmployeeID { get; set; }
        public int? UserID { get; set; }
        public int BranchID { get; set; }
        public int PositionID { get; set; }
        public int DepartmentID { get; set; }
        public DateOnly? HireDate { get; set; }

        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime UpdatedOn { get; set; }
        //public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        // Branch
        public string BranchName { get; set; } = null!;
        public string Location { get; set; } = null!;

        // Position
        public string PositionName { get; set; } = null!;

        // Department
        public string DeparmentName { get; set; } = null!;
    }
}
