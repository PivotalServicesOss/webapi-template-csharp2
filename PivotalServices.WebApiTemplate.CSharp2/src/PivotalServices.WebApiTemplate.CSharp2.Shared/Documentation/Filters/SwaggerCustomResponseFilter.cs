using Swashbuckle.AspNetCore.Annotations;

namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

/// <summary>
/// Created to include SwaggerCustomResponseAttribute from controller level in addition to action methods
/// </summary>
public class SwaggerCustomResponseFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        IEnumerable<object> controllerAttributes = Array.Empty<object>();
        IEnumerable<object> actionAttributes = Array.Empty<object>();
        IEnumerable<object> metadataAttributes = Array.Empty<object>();

        if (context == null)
            return;

        if (context.MethodInfo != null && context.MethodInfo.DeclaringType != null)
        {
            controllerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            actionAttributes = context.MethodInfo.GetCustomAttributes(true);
        }

        if (context.ApiDescription?.ActionDescriptor?.EndpointMetadata != null)
            metadataAttributes = context.ApiDescription.ActionDescriptor.EndpointMetadata;

        // NOTE: When controller and action attributes are applicable, action attributes should take precendence.
        // Hence why they're at the end of the list (i.e. last one wins).
        // Distinct() is applied due to an ASP.NET Core issue: https://github.com/dotnet/aspnetcore/issues/34199.
        var allAttributes = controllerAttributes
            .Union(actionAttributes)
            .Union(metadataAttributes)
            .Distinct();

        ApplySwaggerCustomResponseAttributes(operation, context, allAttributes);
    }

    private void ApplySwaggerCustomResponseAttributes(OpenApiOperation operation,
                                                OperationFilterContext context,
                                                IEnumerable<object> controllerAndActionAttributes)
    {
        var swaggerResponseAttributes = controllerAndActionAttributes.OfType<SwaggerCustomResponseAttribute>();

        foreach (var swaggerResponseAttribute in swaggerResponseAttributes)
        {
            var statusCode = swaggerResponseAttribute.StatusCode.ToString();

            if (operation.Responses == null)
                operation.Responses = new OpenApiResponses();

            if (!operation.Responses.TryGetValue(statusCode, out OpenApiResponse? response))
                response = new OpenApiResponse();

            if (swaggerResponseAttribute.Description != null)
                response.Description = swaggerResponseAttribute.Description;

            var headerType = swaggerResponseAttribute.HeaderType;

            if (headerType != null)
            {
                response.Headers ??= new Dictionary<string, OpenApiHeader>();

                foreach (var propertyInfo in headerType.GetProperties())
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    var returnType = underlyingType ?? propertyInfo.PropertyType;

                    string? description = null;

                    var schemaAttributes = propertyInfo.GetCustomAttributes(true).OfType<SwaggerSchemaAttribute>();

                    foreach (var schemaAttribute in schemaAttributes)
                        description = schemaAttribute.Description;

                    var openApiHeader = new OpenApiHeader()
                    {
                        Description = description,
                        Schema = new OpenApiSchema()
                        {
                            Type = returnType.Name,
                        }
                    };

                    if (response.Headers.ContainsKey(propertyInfo.Name) is false)
                        response.Headers.Add(propertyInfo.Name, openApiHeader);
                }
            }

            operation.Responses[statusCode] = response;

            if (swaggerResponseAttribute.ContentTypes != null)
            {
                response.Content.Clear();

                foreach (var contentType in swaggerResponseAttribute.ContentTypes)
                {
                    var schema = (swaggerResponseAttribute.Type != null && swaggerResponseAttribute.Type != typeof(void))
                        ? context.SchemaGenerator.GenerateSchema(swaggerResponseAttribute.Type, context.SchemaRepository)
                        : null;

                    response.Content.Add(contentType, new OpenApiMediaType { Schema = schema });
                }
            }
        }
    }
}