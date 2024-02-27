
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentity.Requirements
{
    public class ExpireDateRequirement : IAuthorizationRequirement
    {

    }

    public class ExpireDateRequirementHandler : AuthorizationHandler<ExpireDateRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpireDateRequirement requirement)
        {
            
            var expiraDate = context.User.Claims.FirstOrDefault(x => x.Type == "ExpireDateOfFreeAccess")?.Value;
            
            if ( expiraDate == null || DateTime.Now >= Convert.ToDateTime(expiraDate) ) context.Fail();
            else context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}
