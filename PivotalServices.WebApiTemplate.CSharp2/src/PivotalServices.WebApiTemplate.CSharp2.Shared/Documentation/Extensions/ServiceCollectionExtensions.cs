namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiInfoOptions>(configuration.GetSection("ApiInfo"));

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            foreach (var xmlCommentsFilePath in XmlCommentsFilePaths)
                options.IncludeXmlComments(xmlCommentsFilePath, true);

            options.EnableAnnotations(true, true);
            
            options.OperationFilter<SwaggerDefaultValuesFilter>();
            options.OperationFilter<SwaggerCustomResponseFilter>();
            options.SchemaFilter<SwaggerSchemaExampleFilter>();

            options.CustomSchemaIds(x => x.FullName);
            options.UseAllOfToExtendReferenceSchemas();
        });

        return services;
    }

    static IList<string> XmlCommentsFilePaths
    {
        get
        {
            var xmlCommentsFilePaths = new List<string>();
            var basePath = System.AppContext.BaseDirectory;
            var commentFiles = Directory.GetFiles(basePath, "PivotalServices.WebApiTemplate.CSharp2.*.xml");
            foreach (var file in commentFiles)
                xmlCommentsFilePaths.Add(file);
            return xmlCommentsFilePaths;
        }
    }
}
