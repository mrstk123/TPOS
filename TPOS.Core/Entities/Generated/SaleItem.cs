﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Domain.Entities.Generated;

public partial class SaleItem
{
    public int SaleItemID { get; set; }
    public int SaleID { get; set; }
    public int ProductID { get; set; }
    public int? ProductItemID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int? TaxID { get; set; }
    public decimal? TaxAmount { get; set; }
    public int? DiscountID { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual Discount Discount { get; set; }
        
    [JsonIgnore]
    public virtual Product Product { get; set; }
        
    [JsonIgnore]
    public virtual ProductItem ProductItem { get; set; }
        
    [JsonIgnore]
    public virtual Sale Sale { get; set; }
        
    [JsonIgnore]
    public virtual Tax Tax { get; set; }
}
