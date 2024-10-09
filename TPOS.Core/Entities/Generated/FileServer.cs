﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TPOS.Domain.Entities.Generated;

public partial class FileServer
{
    public int FileServerID { get; set; }
    public string ServerName { get; set; }
    public string PhysicalPath { get; set; }
    public string VirtualPath { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public bool Active { get; set; }
        
    [JsonIgnore]
    public virtual ICollection<Folder> Folders { get; set; }
}
