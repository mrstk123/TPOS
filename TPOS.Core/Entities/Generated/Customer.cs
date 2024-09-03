﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Core.Entities.Generated
{
    public partial class Customer
    {
        public int CustomerID { get; set; }
        public int? UserID { get; set; }
        public int ContactID { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual ContactInfo Contact { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Loyalty> Loyalties { get; set; }

        [JsonIgnore]
        public virtual ICollection<Sale> Sales { get; set; }

    }
}
