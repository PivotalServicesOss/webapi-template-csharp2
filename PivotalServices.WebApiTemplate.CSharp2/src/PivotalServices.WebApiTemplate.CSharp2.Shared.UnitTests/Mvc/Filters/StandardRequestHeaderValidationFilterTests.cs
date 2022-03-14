public class StandardRequestHeaderValidationFilterTests
{
    readonly StandardRequestHeaderValidationFilter filter;
    readonly HttpContext httpContext;
    readonly RouteData routeData;
    readonly ControllerActionDescriptor actionDescriptor;
    readonly ActionContext actionContext;
    readonly List<IFilterMetadata> filtersMetadata;
    readonly Controller controller;

    public int ActionInvokeCount { get; private set; } = 0;

    public StandardRequestHeaderValidationFilterTests()
    {
        filter = new StandardRequestHeaderValidationFilter();
        httpContext = Mock.Of<HttpContext>();
        routeData = Mock.Of<RouteData>();
        actionDescriptor = Mock.Of<ControllerActionDescriptor>();
        actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
        filtersMetadata = new List<IFilterMetadata>();
        controller = Mock.Of<Controller>();
    }

    [Fact]
    public void Should_BeAssignableTo_IAsyncActionFilter()
    {
        filter.Should().BeAssignableTo<IAsyncActionFilter>();
    }

    [Fact]
    public async Task OnActionExecutionAsync_GivenEmptyHeader_Should_ThrowValidationException()
    {
        //Arrange 
        var standardRequestHeader = new StandardRequestHeader();

        var actionArguments = new Dictionary<string, object?>
        {
            {
                "standardRequestHeader", standardRequestHeader
            }
        };

        var context = new ActionExecutingContext(actionContext, filtersMetadata, actionArguments, controller);

        //Act
        var validationException = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                                        () => filter.OnActionExecutionAsync(context, TestDelegate));

        //Assert
        validationException.Errors.Should().NotBeNullOrEmpty();
        validationException.Errors.Count().Should().Be(2);

        var correlationIdNotFoundError = validationException.Errors.SingleOrDefault(e => e.PropertyName == "CorrelationId");
        correlationIdNotFoundError.Should().NotBeNull();

        var requestDateTimeNotFoundError = validationException.Errors.SingleOrDefault(e => e.PropertyName == "RequestDateTimeUtc");
        requestDateTimeNotFoundError.Should().NotBeNull();
    }

    [Fact]
    public async Task OnActionExecutionAsync_GivenInvalidHeader_Should_ThrowValidationException()
    {
        //Arrange 
        var standardRequestHeader = new StandardRequestHeader
        {
            CorrelationId = "invalid correlationId",
            RequestDateTimeUtc = "invalid datetime"
        };

        var actionArguments = new Dictionary<string, object?>
        {
            {
                "standardRequestHeader", standardRequestHeader
            }
        };

        var context = new ActionExecutingContext(actionContext, filtersMetadata, actionArguments, controller);

        //Act
        var validationException = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => filter.OnActionExecutionAsync(context, TestDelegate));

        //Assert
        validationException.Errors.Should().NotBeNullOrEmpty();
        validationException.Errors.Count().Should().Be(2);

        var correlationIdNotFoundError = validationException.Errors.SingleOrDefault(e => e.PropertyName == "CorrelationId");
        correlationIdNotFoundError.Should().NotBeNull();

        var requestDateTimeNotFoundError = validationException.Errors.SingleOrDefault(e => e.PropertyName == "RequestDateTimeUtc");
        requestDateTimeNotFoundError.Should().NotBeNull();
    }

    [Fact]
    public async Task OnActionExecutionAsync_GivenValidHeader_Should_ExecuteTheActionDelegate()
    {
        //Arrange 
        var standardRequestHeader = new StandardRequestHeader
        {
            CorrelationId = "f8614127-976e-4fc1-bfaa-c92a8d9911fd",
            RequestDateTimeUtc = "2999-12-31T00:00:00.000Z"
        };

        var actionArguments = new Dictionary<string, object?>
        {
            {
                "standardRequestHeader", standardRequestHeader
            }
        };

        var context = new ActionExecutingContext(actionContext, filtersMetadata, actionArguments, controller);

        //Act
        await filter.OnActionExecutionAsync(context, TestDelegate);

        //Assert
        ActionInvokeCount.Should().Be(1);
    }

    private Task<ActionExecutedContext> TestDelegate()
    {
        ActionInvokeCount++;
        return Task.FromResult(new ActionExecutedContext(actionContext, filtersMetadata, controller));
    }
}
