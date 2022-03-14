namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class ResponseHeaderMiddleware
{
    readonly RequestDelegate next;

    public ResponseHeaderMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.Headers.TryGetValue("CorrelationId", out var correlationId);

        context.Response.Headers.Add("CorrelationId", correlationId);
        context.Response.Headers.Add("ResponseDateTimeUtc", $"{DateTimeOffset.Now:yyyy-MM-ddTHH:mm:ss.FFFZ}");

        await next.Invoke(context);
    }
}