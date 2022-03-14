using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class ErrorHandlerMiddleware
{
    readonly RequestDelegate next;
    private readonly IWebHostEnvironment environment;
    readonly ILogger<ErrorHandlerMiddleware> logger;
    readonly HashSet<string> allowedHeaderNames;

    public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<ErrorHandlerMiddleware> logger)
    {
        this.next = next;
        this.environment = environment;
        this.logger = logger;
        allowedHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,
            HeaderNames.StrictTransportSecurity,
            HeaderNames.WWWAuthenticate,
            "CorrelationId",
            "ResponseDateTimeUtc",
        };
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);

            switch (context.Response?.StatusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    throw new UnauthorizedAccessException(StatusDescription.UnAuthorized);
                case StatusCodes.Status403Forbidden:
                    throw new UnauthorizedAccessException(StatusDescription.Forbidden);
            }
        }
        catch (FluentValidation.ValidationException exception)
        {
            logger.ValidationException(exception.Message, exception);

            var standardResponse = new StandardErrorResponse
            {
                Status = Status.Failed,
                StatusDetails = new List<StatusDetail>()
            };

            foreach (var e in exception.Errors)
            {
                standardResponse.StatusDetails.Add(
                    new StatusDetail
                    {
                        FieldName = e.PropertyName,
                        Description = e.ErrorMessage,
                    });
            };

            await WriteErrorResponse(exception, context, HttpStatusCode.BadRequest,
                    Status.Failed, string.Empty, standardResponse);
        }
        catch (UnauthorizedAccessException exception)
        {
            logger.UnauthorizedAccessException(exception.Message, exception);
            await WriteErrorResponse(exception, context, HttpStatusCode.Unauthorized,
                    Status.AuthFailure, exception.Message);
        }
        catch (ApplicationException exception)
        {
            logger.ApplicationException(exception.Message, exception);
            await WriteErrorResponse(exception, context, HttpStatusCode.InternalServerError,
                    Status.Error, StatusDescription.Error);
        }
        catch (Exception exception)
        {
            logger.Exception(exception.Message, exception);
            await WriteErrorResponse(exception, context, HttpStatusCode.InternalServerError,
                    Status.Error, StatusDescription.Error);
        }
    }

    private async Task WriteErrorResponse(Exception exception,
                                            HttpContext context,
                                            HttpStatusCode statusCode,
                                            Status status,
                                            string statusDescription,
                                            StandardErrorResponse? standardResponse = null)
    {
        InitializeResponse(context, statusCode);

        standardResponse = standardResponse ?? new StandardErrorResponse
        {
            Status = status,
            StatusDetails = new List<StatusDetail>
            {
                new StatusDetail
                {
                    Description = statusDescription,
                    ProblemDetails = GetProblemDetails(exception, context),
                }
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(standardResponse,
                                            SerializationHelper.GetSerializerOptions()));
    }

    private ProblemDetails? GetProblemDetails(Exception exception, HttpContext context)
    {
        if(environment.EnvironmentName.ToLower() == "production")
            return null;

        var problemDetail = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Type = $"https://httpstatuses.com/{context.Response.StatusCode}",
            Title = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
            Detail = exception.Message,
        };

        problemDetail.Extensions.Add("exception",
            new Dictionary<string, string>()
            {
                { "type", nameof(exception) },
                { "message", exception.Message },
                { "source", exception.Source ?? "null" },
                { "stackTrace", exception.StackTrace ?? "null"},
            }
        );
        return problemDetail;
    }

    private void InitializeResponse(HttpContext context, HttpStatusCode statusCode)
    {
        var headers = new HeaderDictionary();

        foreach (var header in context.Response.Headers)
        {
            if (allowedHeaderNames.Contains(header.Key))
                headers.Add(header);
        }

        context.Response.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)statusCode;

        foreach (var header in headers)
            context.Response.Headers.Add(header);
    }
}