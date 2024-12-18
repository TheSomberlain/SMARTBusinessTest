using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SMARTBusinessTest.Application.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SMARTBusinessTest.Web.Filters;

namespace SMARTBusinessTest.UnitTests.Filters
{
    [TestFixture]
    public class ApiAuthFilterTests
    {
        private IConfiguration _configuration;
        private ApiAuthFilter _apiAuthFilter;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { AuthConstants.ApiKeySectionName, "validApiKey" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _apiAuthFilter = new ApiAuthFilter(_configuration);
        }

        [Test]
        public async Task OnAuthorizationAsync_WhenApiKeyIsMissing_ThenShouldReturnUnauthorized()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var context = CreateAuthFilterContext(httpContext);

            // Act
            await _apiAuthFilter.OnAuthorizationAsync(context);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(context.Result);
            var result = (UnauthorizedObjectResult)context.Result;
            Assert.That(result.Value, Is.EqualTo(AuthConstants.ApiKeyMissing));
        }

        [Test]
        public async Task OnAuthorizationAsync_WhenApiKeyIsInvalid_ThenShouldReturnUnauthorized()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[AuthConstants.ApiKeyHeaderName] = "invalidApiKey";
            var context = CreateAuthFilterContext(httpContext);

            // Act
            await _apiAuthFilter.OnAuthorizationAsync(context);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(context.Result);
            var result = (UnauthorizedObjectResult)context.Result;
            Assert.That(result.Value, Is.EqualTo(AuthConstants.InvalidApiKey));
        }

        [Test]
        public async Task OnAuthorizationAsync_WhenApiKeyIsValid_ThenShouldNotReturnResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[AuthConstants.ApiKeyHeaderName] = "validApiKey";
            var context = CreateAuthFilterContext(httpContext);

            // Act
            await _apiAuthFilter.OnAuthorizationAsync(context);

            // Assert
            Assert.IsNull(context.Result);
        }

        private AuthorizationFilterContext CreateAuthFilterContext(HttpContext context)
        {
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = context,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };
            return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
        }
    }
}
