
using Microsoft.AspNetCore.Hosting;

public class ErrorHandlerMiddlewareTests
{
    readonly Mock<ILogger<ErrorHandlerMiddleware>> logger;
    readonly Mock<IWebHostEnvironment> environment;

    public ErrorHandlerMiddlewareTests()
    {
        logger = new Mock<ILogger<ErrorHandlerMiddleware>>();
        environment = new Mock<IWebHostEnvironment>();
        environment.SetupGet(e => e.EnvironmentName).Returns("development");
    }

    [Fact]
    public async Task Invoke_Should_HandleValidationException()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(ThrowValidationException, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{FieldName = "Property1", Description = "Property1 is empty"},
            new StatusDetail{FieldName = "Property2", Description = "Property2 is invalid"}
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status400BadRequest, Status.Failed);

        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public async Task Invoke_Should_HandleUnhandledException()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(ThrowUnhandledException, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{ Description = StatusDescription.Error }
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status500InternalServerError, Status.Error);
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public async Task Invoke_Should_HandleUnauthorizedException()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(ThrowUnauthorizedException, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{ Description = StatusDescription.UnAuthorized }
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status401Unauthorized, Status.AuthFailure);
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public async Task Invoke_Should_HandleForbiddenContext()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(SetForbidden, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{ Description = StatusDescription.Forbidden }
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status401Unauthorized, Status.AuthFailure);
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public async Task Invoke_Should_HandleUnAuthorizedContext()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(SetUnauthorized, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{ Description = StatusDescription.UnAuthorized }
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status401Unauthorized, Status.AuthFailure);
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Fact]
    public async Task Invoke_Should_HandleApplicationException()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ErrorHandlerMiddleware(ThrowApplicationException, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var expectedStatusDetails = new List<StatusDetail>
        {
            new StatusDetail{ Description = StatusDescription.Error },
        };

        await AssertResponse(context, expectedStatusDetails, StatusCodes.Status500InternalServerError, Status.Error);
        logger.Verify(logger => logger.IsEnabled(LogLevel.Error), Times.Once);
    }

    [Theory]
    [InlineData("Production", false)]
    [InlineData("Development", true)]
    public async Task Invoke_Should_AddProblemDetailsBasedOnEnvironmentName(string environmentName, bool isProblemDetailsAvailable)
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        environment.SetupGet(e => e.EnvironmentName).Returns(environmentName);
        var middleware = new ErrorHandlerMiddleware(ThrowApplicationException, environment.Object, logger.Object);

        //Act
        await middleware.Invoke(context);

        //Assert
        var reader = new StreamReader(context.Response.Body);
        context.Response.Body.Position = 0;
        var content = await reader.ReadToEndAsync();

        content.Should().NotBeEmpty();

        var standardResponse = JsonSerializer.Deserialize<StandardErrorResponse>(content, SerializationHelper.GetSerializerOptions());
        standardResponse?.StatusDetails?.Any(sd => sd.ProblemDetails != null).Should().Be(isProblemDetailsAvailable);
    }

    private async Task AssertResponse(DefaultHttpContext context,
                                        List<StatusDetail> expectedStatusDetails,
                                        int statusCode,
                                        Status status)
    {
        context.Response.StatusCode.Should().Be(statusCode);
        context.Response.ContentType.Should().Be(MediaTypeNames.Application.Json);

        var reader = new StreamReader(context.Response.Body);
        context.Response.Body.Position = 0;
        var content = await reader.ReadToEndAsync();

        content.Should().NotBeEmpty();

        var standardResponse = JsonSerializer.Deserialize<StandardErrorResponse>(content, SerializationHelper.GetSerializerOptions());

        standardResponse.Should().NotBeNull();
        standardResponse?.Status.Should().Be(status);
        standardResponse?.StatusDetails.Should().BeEquivalentTo(expectedStatusDetails, opt => opt.Excluding(e => e.ProblemDetails));
    }

    private Task ThrowValidationException(HttpContext context)
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Property1", "Property1 is empty"),
                new ValidationFailure("Property2", "Property2 is invalid"),
            };
        throw new FluentValidation.ValidationException(failures);
    }

    private Task ThrowUnhandledException(HttpContext context)
    {
        throw new Exception("Dummy Application Exception");
    }

    private Task ThrowApplicationException(HttpContext context)
    {
        throw new ApplicationException("Some ApplicationException occurred");
    }

    private Task ThrowUnauthorizedException(HttpContext context)
    {
        throw new UnauthorizedAccessException(StatusDescription.UnAuthorized);
    }

    private async Task SetUnauthorized(HttpContext context)
    {
        await Task.FromResult(context.Response.StatusCode = StatusCodes.Status401Unauthorized);
    }

    private async Task SetForbidden(HttpContext context)
    {
        await Task.FromResult(context.Response.StatusCode = StatusCodes.Status403Forbidden);
    }
}