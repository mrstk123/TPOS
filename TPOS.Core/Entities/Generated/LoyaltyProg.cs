﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Domain.Entities.Generated;

public partial class LoyaltyProg
{
    public int LoyaltyProgID { get; set; }
    public string LoyaltyProgName { get; set; }
    public string LoyaltyProgDescription { get; set; }
    public decimal PointsPerAmount { get; set; }
    public decimal? PointsMultiplier { get; set; }
    public DateTime? Validity { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual ICollection<Loyalty> Loyalties { get; set; }
}
