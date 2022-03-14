public class LoggerExtensionsTests
{
    readonly Mock<ILogger<TestClass>> logger;
    TestClass tester;
    public LoggerExtensionsTests()
    {
        logger = new Mock<ILogger<TestClass>>();
        tester = new TestClass(logger.Object);
    }

    [Fact]
    public void Should_EnableInfoLogger()
    {
        tester.LogInfo();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Information), Times.Once);
    }

    [Fact]
    public void Should_EnableDebugLogger()
    {
        tester.LogDebug();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Debug), Times.Once);
    }

    [Fact]
    public void Should_EnableWarningLogger()
    {
        tester.LogWarning();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Warning), Times.Once);
    }

    [Fact]
    public void Should_EnableExceptionLogger()
    {
        tester.LogException();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public void Should_EnableApplicationExceptionLogger()
    {
        tester.LogApplicationException();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public void Should_EnableValidationExceptionLogger()
    {
        tester.LogValidationException();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public void Should_EnableUnauthorizedAccessExceptionLogger()
    {
        tester.LogUnauthorizedAccessException();
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }
}

public class TestClass
{
    readonly ILogger<TestClass> logger;

    public TestClass(ILogger<TestClass> logger)
    {
        this.logger = logger;
    }

    public void LogInfo()
    {
        logger.Information("some message");
    }

    public void LogDebug()
    {
        logger.Debug("some message");
    }

    public void LogWarning()
    {
        logger.Warning("some message");
    }

    public void LogException()
    {
        logger.Exception("error message", new Exception());
    }

    public void LogApplicationException()
    {
        logger.ApplicationException("error message", new ApplicationException());
    }

    public void LogUnauthorizedAccessException()
    {
        logger.UnauthorizedAccessException("error message", new UnauthorizedAccessException());
    }

    public void LogValidationException()
    {
        logger.ValidationException("error message", new FluentValidation.ValidationException(""));
    }
}