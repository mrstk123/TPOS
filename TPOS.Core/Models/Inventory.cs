﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TPOS.Core.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int BranchId { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public bool Active { get; set; }

    public virtual Branch Branch { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual Product Product { get; set; }

    public virtual User UpdatedByNavigation { get; set; }
}