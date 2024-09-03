﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Core.Entities.Generated
{
    public partial class UserRole
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

    }
}
