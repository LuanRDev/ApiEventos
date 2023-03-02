using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ApiEventos.WebApi.Auth
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IConfiguration _configuration;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if(policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new HasScopeRequirement(policyName, $"{_configuration["Keycloak:UrlBase"]}{_configuration["Keycloak:Authority"]}"))
                    .Build();
                _options.AddPolicy(policyName, policy);
            }
            return policy;
        }
    }
}
