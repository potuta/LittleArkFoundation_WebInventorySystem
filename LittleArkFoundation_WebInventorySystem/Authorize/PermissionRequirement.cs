using Microsoft.AspNetCore.Authorization;

namespace LittleArkFoundation_WebInventorySystem.Authorize
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

}
