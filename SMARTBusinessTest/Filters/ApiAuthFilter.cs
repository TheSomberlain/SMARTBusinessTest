using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SMARTBusinessTest.Application.Constants;

namespace SMARTBusinessTest.Web.Filters
{
    public class ApiAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var expectedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(AuthConstants.ApiKeyMissing);
                return;
            }
            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if (apiKey != expectedApiKey)
            {
                context.Result = new UnauthorizedObjectResult(AuthConstants.InvalidApiKey);
                return;
            }
        }
    }
}
