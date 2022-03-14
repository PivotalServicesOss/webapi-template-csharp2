namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception?> infoAction;
    private static readonly Action<ILogger, string, Exception?> debugAction;
    private static readonly Action<ILogger, string, Exception?> warningAction;
    private static readonly Action<ILogger, string, FluentValidation.ValidationException> validationExeceptionAction;
    private static readonly Action<ILogger, string, UnauthorizedAccessException> unauthorizedAccessExceptionAction;
    private static readonly Action<ILogger, string, Exception> exceptionAction;
    private static readonly Action<ILogger, string, ApplicationException> applicationExceptionAction;

    static LoggerExtensions()
    {
        infoAction = LoggerMessage.Define<string>(LogLevel.Information, new EventId(1000), "{Message}");
        debugAction = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1000), "{Message}");
        warningAction = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1000), "{Message}");

        validationExeceptionAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(1001, nameof(FluentValidation.ValidationException)), "{Message}");
        unauthorizedAccessExceptionAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(1002, nameof(UnauthorizedAccessException)), "{Message}");
        exceptionAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(1003, nameof(Exception)), "{Message}");
        applicationExceptionAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(1004, nameof(ApplicationException)), "{Message}");
    }


    public static void Information(this ILogger logger, string message)
    {
        infoAction(logger, message, null);
    }

    public static void Debug(this ILogger logger, string message)
    {
        debugAction(logger, message, null);
    }

    public static void Warning(this ILogger logger, string message)
    {
        warningAction(logger, message, null);
    }

    public static void ValidationException(this ILogger logger, string message, FluentValidation.ValidationException exception)
    {
        validationExeceptionAction(logger, message, exception);
    }

    public static void UnauthorizedAccessException(this ILogger logger, string message, UnauthorizedAccessException exception)
    {
        unauthorizedAccessExceptionAction(logger, message, exception);
    }

    public static void Exception(this ILogger logger, string message, Exception exception)
    {
        exceptionAction(logger, message, exception);
    }

    public static void ApplicationException(this ILogger logger, string message, ApplicationException exception)
    {
        applicationExceptionAction(logger, message, exception);
    }
}
