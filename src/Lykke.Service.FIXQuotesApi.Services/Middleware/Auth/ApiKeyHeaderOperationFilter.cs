using System.Collections.Generic;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace Lykke.Service.FIXQuotesApi.Services.Middleware.Auth
{
    public class ApiKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;

            operation.Parameters = operation.Parameters ?? new List<IParameter>();
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "api-key",
                In = "header",
                Description = "An API access token",
                Required = false,
                Type = "string"
            });
        }
    }
}
