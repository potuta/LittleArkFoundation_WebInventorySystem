﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LittleArkFoundation_WebInventorySystem.Authorize
{
    public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public HasPermissionAttribute(string permission) 
        { 
            _permission = permission; 
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasPermission = user.Claims.Any(c => c.Type == "Permission" && c.Value == _permission);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
