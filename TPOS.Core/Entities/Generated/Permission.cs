﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Domain.Entities.Generated;

public partial class Permission
{
    public int PermissionID { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public string Value { get; set; }
    public string PermissionModule { get; set; }
    public string PermissionGroup { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual ICollection<Menu> Menus { get; set; }
        
    [JsonIgnore]
    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}
