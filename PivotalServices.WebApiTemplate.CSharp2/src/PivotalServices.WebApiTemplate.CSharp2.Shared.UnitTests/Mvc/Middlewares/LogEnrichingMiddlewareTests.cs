using Microsoft.AspNetCore.Hosting;

public class LogEnrichingMiddlewareTests
{
    public int InvokeCount { get; private set; }

    [Fact]
    public async Task Invoke_Should_CallNextMiddleware()
    {
        //Arrange
        var context = new DefaultHttpContext();
        
        var environment = new Mock<IWebHostEnvironment>();
        var middleware = new LogEnrichingMiddleware(ProcessRequestDelegate, environment.Object);

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