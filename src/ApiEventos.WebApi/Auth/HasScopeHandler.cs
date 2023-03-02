using Microsoft.AspNetCore.Authorization;

namespace ApiEventos.WebApi.Auth
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement scopeRequirements)
        {
            if (!context.User.HasClaim(c => c.Type == "resourse_access"))
            {
                return Task.CompletedTask;
            }

            var scopes = context.User.FindFirst(c => c.Type == "resource_access").Value;
            string[] requirements = scopeRequirements.Scope.Split(',');
            bool hasAllRequirements = requirements.All(requirement => scopes.Contains(requirement));
            if(hasAllRequirements)
            {
                context.Succeed(scopeRequirements);
            }
            return Task.CompletedTask;
        }
    }
}
