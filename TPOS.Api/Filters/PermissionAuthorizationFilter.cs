﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TPOS.Application.Interfaces;

public class PermissionAuthorizationFilter: IAuthorizationFilter
{
    private readonly string _permission;
    private readonly IUnitOfWork _unitOfWork;

    public PermissionAuthorizationFilter(string permission, IUnitOfWork unitOfWork)
    {
        _permission = permission;
        _unitOfWork = unitOfWork;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userID))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = _unitOfWork.UserRepository.HasPermission(userID, _permission);
        if (!hasPermission)
        {
            context.Result = new ForbidResult(); // Or UnauthorizedResult  
        }
    }
}

