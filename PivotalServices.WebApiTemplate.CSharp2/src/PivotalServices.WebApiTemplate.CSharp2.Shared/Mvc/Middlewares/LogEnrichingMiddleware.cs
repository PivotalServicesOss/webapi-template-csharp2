using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Context;

namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class LogEnrichingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment environment;

    public LogEnrichingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        this.next = next;
        this.environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        var containsCorrelationHeader = context.Request.Headers.TryGetValue("CorrelationId", out var _correlationId);

        using (LogContext.PushProperty("Enviroment", environment.EnvironmentName))
        using (LogContext.PushProperty("ApplicationName", environment.ApplicationName))
        using (LogContext.PushProperty("ApplicationVersion", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version))
        using (LogContext.PushProperty("CorrelationId", containsCorrelationHeader ? _correlationId : "null"))
        {
            await next(context);
        }
    }
}