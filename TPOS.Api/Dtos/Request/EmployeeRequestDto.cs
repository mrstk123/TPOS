namespace TPOS.Api.Dtos.Request
{
    public class EmployeeRequestDto: ContactInfoDto
    {
        public int EmployeeID { get; set; }
        public int? UserID { get; set; }
        public int BranchID { get; set; }
        public int PositionID { get; set; }
        public int DepartmentID { get; set; }
        public DateOnly? HireDate { get; set; }
        public bool Active { get; set; }
    }
}
