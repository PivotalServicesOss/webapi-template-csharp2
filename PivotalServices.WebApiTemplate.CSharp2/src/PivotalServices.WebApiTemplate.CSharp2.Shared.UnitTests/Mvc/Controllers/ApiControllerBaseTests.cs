
public class ApiControllerBaseTests
{
    [Fact]
    public void ApiControllerBase_Should_Inherit_ApiControllerBase()
    {
        typeof(ApiControllerBase).Should().BeAssignableTo<ControllerBase>();
    }

    [Fact]
    public void ApiControllerBase_Should_Use_The_StandardHeaderValidationFilter()
    {
        typeof(ApiControllerBase).Should()
            .BeDecoratedWith<ServiceFilterAttribute>(s =>
                s.ServiceType == typeof(StandardRequestHeaderValidationFilter));
    }

    [Fact]
    public void ApiControllerBase_Should_Use_ApiControllerAttribute()
    {
        typeof(ApiControllerBase).Should()
            .BeDecoratedWith<ApiControllerAttribute>();
    }

    [Fact]
    public void ApiControllerBase_Should_Use_RouteAttribute()
    {
        typeof(ApiControllerBase).Should()
            .BeDecoratedWith<RouteAttribute>(r => r.Template.Equals("v{version:apiVersion}/[controller]"));
    }

    [Fact]
    public void ApiControllerBase_Should_Use_ConsumesAttribute_With_Json()
    {
        typeof(ApiControllerBase).Should()
            .BeDecoratedWith<ConsumesAttribute>(r => r.ContentTypes.Contains(MediaTypeNames.Application.Json));
    }

    [Fact]
    public void ApiControllerBase_Should_Apply_ProducesCustomResponseType_Decorators()
    {
        foreach (var code in new[] { 400, 401, 500 })
        {
            typeof(ApiControllerBase).Should()
                    .BeDecoratedWith<SwaggerCustomResponseAttribute>(p =>
                        p.StatusCode.Equals(code) && p.Type.IsAssignableTo(typeof(StandardErrorResponse)), 
                        $"required SwaggerCustomResponseAttribute(statusCode: {code}, type: typeof(StandardErrorResponse))");
        }
    }
}