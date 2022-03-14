using Serilog;

namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

[ExcludeFromCodeCoverage]
public static class BuilderExtensions
{
    /// <summary>
    /// This should be called in the begening of the pipeline
    /// </summary>
    public static IApplicationBuilder AddGlobalMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogEnrichingMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<ResponseHeaderMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();

        return app;
    }

    public static IEndpointRouteBuilder UseMvcPipeline(this IEndpointRouteBuilder app)
    {
        app.MapControllers();

        return app;
    }
}
