using Microsoft.OpenApi.Models;
using SMARTBusinessTest.Application.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SMARTBusinessTest.Web.Filters
{
    public class SwaggerAuthFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuthConstants.ApiKeyHeaderName,
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}
