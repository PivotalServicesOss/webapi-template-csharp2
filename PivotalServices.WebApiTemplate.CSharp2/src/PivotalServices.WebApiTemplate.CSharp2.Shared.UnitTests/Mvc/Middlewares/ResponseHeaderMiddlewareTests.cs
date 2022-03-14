public class ResponseHeaderMiddlewareTests
{
    public int InvokeCount { get; private set; }

    [Fact]
    public async Task Invoke_Should_CreateStandardResponseHeader()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers.Add("CorrelationId", "e33e0003-26af-4409-9896-b83aa0c7c8ea");

        //Act
        var middleware = new ResponseHeaderMiddleware(ProcessRequestDelegate);
        await middleware.Invoke(context);

        //Assert
        context.Response.Headers.Keys.Should().Contain("CorrelationId");
        context.Response.Headers.Keys.Should().Contain("ResponseDateTimeUtc");
    }

    [Fact]
    public async Task ResponseDateTimeUtc_Should_BeInUtcFormat()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers.Add("CorrelationId", "e33e0003-26af-4409-9896-b83aa0c7c8ea");

        //Act
        var middleware = new ResponseHeaderMiddleware(ProcessRequestDelegate);
        await middleware.Invoke(context);

        //Assert
        context.Response.Headers.TryGetValue("ResponseDateTimeUtc", out var responseDateTimeUtc);

        (DateTime.TryParse(responseDateTimeUtc, null,
                DateTimeStyles.AdjustToUniversal,
                out DateTime date) && date.Kind == DateTimeKind.Utc).Should().BeTrue();

        var utcFormatExpression = new Regex("(\\d{4})-(\\d{2})-(\\d{2})T(\\d{2}):(\\d{2}):(\\d{2})(.(\\d{1,3}))?Z");
        utcFormatExpression.IsMatch(responseDateTimeUtc).Should().BeTrue($"{responseDateTimeUtc} is not matching {utcFormatExpression}");
    }

    [Fact]
    public async Task Invoke_Should_CallNextMiddleware()
    {
        //Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        context.Request.Headers.Add("CorrelationId", "e33e0003-26af-4409-9896-b83aa0c7c8ea");

        var middleware = new ResponseHeaderMiddleware(ProcessRequestDelegate);

        //Act
        await middleware.Invoke(context);

        //Assert
        InvokeCount.Should().BeGreaterThan(0);
    }

    private Task ProcessRequestDelegate(HttpContext context)
    {
        InvokeCount++;
        return Task.CompletedTask;
    }
}