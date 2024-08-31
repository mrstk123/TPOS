﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TPOS.Core.Models;

public partial class CurrencyRate
{
    public int CurrencyRateId { get; set; }

    public int BaseCurrencyId { get; set; }

    public int ForeignCurrencyId { get; set; }

    public decimal Rate { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public bool Active { get; set; }

    public virtual Object BaseCurrency { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual Object ForeignCurrency { get; set; }

    public virtual User UpdatedByNavigation { get; set; }
}