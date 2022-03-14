
namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

[ExcludeFromCodeCoverage]
public static class BuilderExtensions
{
    public static IApplicationBuilder UseOpenApiSwaggerDocumentation(this IApplicationBuilder app)
    {
        var apiVersionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                options.RoutePrefix = string.Empty;
            });

        return app;
    }
}
