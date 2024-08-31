﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TPOS.Core.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? UserId { get; set; }

    public int StoreId { get; set; }

    public int ContactId { get; set; }

    public int PositionId { get; set; }

    public int DepartmentId { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public bool Active { get; set; }

    public virtual ContactInfo Contact { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual Object Department { get; set; }

    public virtual Object Position { get; set; }

    public virtual Branch Store { get; set; }

    public virtual User UpdatedByNavigation { get; set; }

    public virtual User User { get; set; }
}