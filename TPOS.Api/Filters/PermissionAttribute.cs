// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using Microsoft.AspNetCore.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(string permission) : base(typeof(PermissionAuthorizationFilter))
    {
        Arguments = new object[] { permission };
    }
}
