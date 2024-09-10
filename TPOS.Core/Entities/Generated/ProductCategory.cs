﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Core.Entities.Generated;

public partial class ProductCategory
{
    public int ProductCategoryID { get; set; }
    public int ProductID { get; set; }
    public int CategoryID { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual Category Category { get; set; }
        
    [JsonIgnore]
    public virtual Product Product { get; set; }
}
