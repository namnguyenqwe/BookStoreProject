using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BookStoreProject.PolicyHandlers
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string _permission { get; set; }
        public PermissionRequirement (string permission)
        {
            _permission = permission;
        }
    }
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private RoleManager<IdentityRole> _roleManager;
        public PermissionHandler (RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
                return Task.CompletedTask;
            IdentityOptions _options = new IdentityOptions();
            var roleValue = context.User.Claims.First(c => c.Type == _options.ClaimsIdentity.RoleClaimType).Value;
            var role = _roleManager.FindByNameAsync(roleValue).GetAwaiter().GetResult();
            var permissionList = _roleManager.GetClaimsAsync(role).GetAwaiter().GetResult();
            var permission = permissionList.FirstOrDefault(x => x.Value.ToUpper() == requirement._permission.ToUpper());
            if (permission != null)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
