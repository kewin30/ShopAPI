using Microsoft.AspNetCore.Authorization;
using ShopAPI.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopAPI.Services
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Order restaurant)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;

        }
    }
}
