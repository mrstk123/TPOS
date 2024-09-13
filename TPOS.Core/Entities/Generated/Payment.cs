﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Domain.Entities.Generated;

public partial class Payment
{
    public int PaymentID { get; set; }
    public int SaleID { get; set; }
    public decimal PaymentAmount { get; set; }
    public int PaymentCurrencyID { get; set; }
    public DateOnly PaymentDate { get; set; }
    public TimeOnly PaymentTime { get; set; }
    public int PaymentTypeID { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual Object PaymentCurrency { get; set; }
        
    [JsonIgnore]
    public virtual Object PaymentType { get; set; }
        
    [JsonIgnore]
    public virtual Sale Sale { get; set; }
}
