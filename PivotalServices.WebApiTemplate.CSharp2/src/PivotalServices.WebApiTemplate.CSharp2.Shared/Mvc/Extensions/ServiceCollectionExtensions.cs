namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMvcPipeline(this IServiceCollection services, IConfiguration configuration, Assembly entryAssembly)
    {
        var appAssemblies = GetReferencedAppAssemblies(entryAssembly);
        appAssemblies.Add(entryAssembly);
        
        services.AddMediatR(appAssemblies.ToArray());
        services.AddValidatorsFromAssemblies(appAssemblies.ToArray());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<StandardRequestHeaderValidationFilter>();

        services.AddEndpointsApiExplorer();

        services.AddControllers(opt => 
        {
            opt.ModelValidatorProviders.Clear();
        })
        .ConfigureApiBehaviorOptions((Action<ApiBehaviorOptions>)(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var validationFailures = GetValidationFailuresFromModelStateErrors(context);
                throw new FluentValidation.ValidationException(validationFailures);
            };
        }))
        .AddJsonOptions(options =>
        {
            var serializerOptions = SerializationHelper.GetSerializerOptions();
            options.JsonSerializerOptions.DefaultIgnoreCondition = serializerOptions.DefaultIgnoreCondition;
            options.JsonSerializerOptions.WriteIndented = serializerOptions.WriteIndented;
            options.JsonSerializerOptions.ReferenceHandler = serializerOptions.ReferenceHandler;

            foreach (var converter in serializerOptions.Converters)
                options.JsonSerializerOptions.Converters.Add(converter);
        });
        return services;
    }

    static List<Assembly> GetReferencedAppAssemblies(Assembly assembly)
    {
        var appAssemblies = new List<Assembly>();

        var referencedAssemblies = assembly.GetReferencedAssemblies();
        var appAssemblyNames = referencedAssemblies
                .Where(e => e.FullName.StartsWith("PivotalServices.WebApiTemplate.CSharp2"));

        foreach (var assemblyName in appAssemblyNames)
            appAssemblies.Add(Assembly.Load(assemblyName));

        return appAssemblies;
    }

    static List<ValidationFailure> GetValidationFailuresFromModelStateErrors(ActionContext context)
    {
        var list = new List<ValidationFailure>();

        foreach (var (key, value) in context.ModelState)
            list.AddRange(value.Errors.Select(error => new ValidationFailure(key, error.ErrorMessage)));

        return list;
    }
}
