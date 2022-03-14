using System.Reflection;
using System.Threading;
using MediatR;
using PivotalServices.WebApiTemplate.CSharp2.Application.Controllers;
using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;
using Swashbuckle.AspNetCore.Annotations;

public class WeatherForecastControllerTests
{
    Mock<IMediator> mediator;
    public WeatherForecastControllerTests()
    {
        mediator = new Mock<IMediator>();
    }

    [Fact]
    public void Controller_ShouldBeOfTypeApiControllerBase()
    {
        Assert.True(new WeatherForecastController(mediator.Object) is ApiControllerBase);
    }

    [Fact]
    public void GetAsync_ShouldBeDecoratedWith()
    {
        var constraint = typeof(WeatherForecastController).Should()
            .HaveMethod(nameof(WeatherForecastController.GetAsync),
                new[]
                {
                    typeof(StandardRequestHeader),
                    typeof(CancellationToken)
                });

        constraint.Which.Should().BeDecoratedWith<HttpGetAttribute>(r => r.Template == "GetAll", "missing HttpGet with GetAll route");

        constraint.Which.Should().BeDecoratedWith<SwaggerOperationAttribute>(s => s.Summary == "Gets weather forecast for all locations", "missing SwaggerOperationAttribute");

        foreach (var code in new[] { 200 })
        {
            constraint.Which.Should().BeDecoratedWith<SwaggerCustomResponseAttribute>(p =>
                    p.StatusCode.Equals(code)
                    && p.Type.IsAssignableTo(typeof(GetWeatherForecastResponse))
                    && p.HeaderType.IsAssignableTo(typeof(StandardResponseHeader))
                    && p.ContentTypes.Contains(MediaTypeNames.Application.Json), $"missing SwaggerCustomResponseAttribute");
        }
    }

    [Fact]
    public void GetAsync_Parameters_ShouldBeDecoratedWith()
    {
        var parameters = typeof(WeatherForecastController).GetMethod("GetAsync", BindingFlags.Instance | BindingFlags.Public)?.GetParameters();

        AssertDecoratedWithSwaggerFromHeader(parameters);
    }

    [Fact]
    public async Task GetAsync_ShouldRespondFromMediator()
    {
        //Arrange
        var header = new StandardRequestHeader();

        var expected = new GetWeatherForecastResponse
        {
            Status = Status.Success,
            WeatherForecasts = new List<GetWeatherForecastDetail> { new GetWeatherForecastDetail() }
        };

        mediator.Setup(m => m.Send<GetWeatherForecastResponse>(
            It.IsAny<GetWeatherForecastRequest>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var controller = new WeatherForecastController(mediator.Object);

        //Act
        var response = await controller.GetAsync(header, default);

        //Assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
    }

    [Fact]
    public void GetByZipCodeAsync_ShouldBeDecoratedWith()
    {
        var constraint = typeof(WeatherForecastController).Should()
            .HaveMethod(nameof(WeatherForecastController.GetByZipCodeAsync),
                new[]
                {
                    typeof(StandardRequestHeader),
                    typeof(GetWeatherForecastByZipCodeRequest),
                    typeof(CancellationToken)
                });

        constraint.Which.Should().BeDecoratedWith<HttpGetAttribute>(r => r.Template == "GetByZipCode", "missing HttpGet with GetByZipCode route");

        constraint.Which.Should().BeDecoratedWith<SwaggerOperationAttribute>(s => s.Summary == "Gets weather forecast for given zipcode", "missing SwaggerOperationAttribute");

        foreach (var code in new[] { 200 })
        {
            constraint.Which.Should().BeDecoratedWith<SwaggerCustomResponseAttribute>(p =>
                    p.StatusCode.Equals(code)
                    && p.Type.IsAssignableTo(typeof(GetWeatherForecastByZipCodeResponse))
                    && p.HeaderType.IsAssignableTo(typeof(StandardResponseHeader))
                    && p.ContentTypes.Contains(MediaTypeNames.Application.Json), $"missing SwaggerCustomResponseAttribute");
        }
    }

    [Fact]
    public void GetByZipCodeAsync_Parameters_ShouldBeDecoratedWith()
    {
        var parameters = typeof(WeatherForecastController).GetMethod("GetByZipCodeAsync", BindingFlags.Instance | BindingFlags.Public)?.GetParameters();

        AssertDecoratedWithSwaggerFromHeader(parameters);

        var parameter = parameters?
                        .FirstOrDefault(p => p.ParameterType.Name == "GetWeatherForecastByZipCodeRequest");
        parameter.Should().NotBeNull("parameter doesn't exist");

        var attributes = parameter?.GetCustomAttributes();
        attributes.Should().NotBeNull("no attribute found");

        attributes?.Any(a => a is FromQueryAttribute)
            .Should()
            .BeTrue("mising FromQueryAttribute");
    }

    [Fact]
    public async Task GetByZipCodeAsync_ShouldRespondFromMediator()
    {
        //Arrange
        var header = new StandardRequestHeader();

        var expected = new GetWeatherForecastByZipCodeResponse
        {
            Status = Status.Success,
            WeatherForecasts = new List<GetWeatherForecastByZipCodeDetail> { new GetWeatherForecastByZipCodeDetail() }
        };

        mediator.Setup(m => m.Send<GetWeatherForecastByZipCodeResponse>(
            It.IsAny<GetWeatherForecastByZipCodeRequest>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var request = new GetWeatherForecastByZipCodeRequest();

        var controller = new WeatherForecastController(mediator.Object);

        //Act
        var response = await controller.GetByZipCodeAsync(header, request, default);

        //Assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;
        var comparer = new ObjectsComparer.Comparer<GetWeatherForecastByZipCodeRequest>();

        mediator.Verify(m => m.Send<GetWeatherForecastByZipCodeResponse>(
                It.Is<GetWeatherForecastByZipCodeRequest>(r => comparer.Compare(request, r)),
                It.IsAny<CancellationToken>()), Times.Once);

        result?.Value.Should().BeSameAs(expected);
    }

    private static void AssertDecoratedWithSwaggerFromHeader(ParameterInfo[] parameters)
    {
        var headerParameter = parameters?
            .FirstOrDefault(p => p.ParameterType.Name == "StandardRequestHeader");
        headerParameter.Should().NotBeNull("parameter doesn't exist");

        var attributes = headerParameter?.GetCustomAttributes();
        attributes.Should().NotBeNull("no attribute found");

        attributes?.Any(a => a is SwaggerFromHeaderAttribute)
            .Should()
            .BeTrue("mising SwaggerFromHeaderAttribute");
    }
}