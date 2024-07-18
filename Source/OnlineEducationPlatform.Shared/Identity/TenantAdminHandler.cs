using Microsoft.AspNetCore.Authorization;
using OnlineEducationPlatform.Shared.Entities;
using System.Security.Claims;

namespace OnlineEducationPlatform.Shared.Identity
{

    public class TenantAdminHandler : AuthorizationHandler<TenantAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantAdminRequirement requirement)
        {
            // Logic to check if user is a Tenant Admin for the requested tenant
            var user = context.User;
            var tenantId = context.Resource as string; // Assuming tenantId is passed as resource

            if (user.IsInRole(Role.SuperAdmin) || (user.IsInRole(Role.TenantAdmin) && IsTenantAdminForTenant(user, tenantId!)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool IsTenantAdminForTenant(ClaimsPrincipal user, string tenantId)
        {
            return true;
        }
    }
}
