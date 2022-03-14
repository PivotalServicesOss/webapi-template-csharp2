using Swashbuckle.AspNetCore.Annotations;

public class SwaggerCustomResponseFilterTests
{
    [Fact]
    public void Should_Output_ResponseHeaders()
    {
        OpenApiOperation operation = new()
        {
            Responses =
                {
                    {"200", new OpenApiResponse { }},
                    {"404", new OpenApiResponse { }},
                    {"401", new OpenApiResponse { }},
                    {"500", new OpenApiResponse { }},
                }
        };
        var methodInfo = typeof(TestControllerWithSwaggerAnnotations)
            .GetMethod(nameof(TestControllerWithSwaggerAnnotations.ActionWithSwaggerResponseAttributes));

        OperationFilterContext filterContext = new(apiDescription: null,
                                                    schemaRegistry: null,
                                                    schemaRepository: null,
                                                    methodInfo: methodInfo);

        Subject().Apply(operation, filterContext);

        Assert.Equal(new[] { "200", "404", "401", "500" }, operation.Responses.Keys.ToArray());

        //Method Level
        var response200 = operation.Responses["200"];
        AssertHeaderSummary(response200);
        response200.Description.Should().Be("<strong>Success</strong><p>Everything worked as expected</p>");

        //Method Level
        var response404 = operation.Responses["404"];
        AssertHeaderSummary(response404);
        response404.Description.Should().Be("<strong>Failure</strong><p>Resource not found</p>");

        //Class Level
        var response422 = operation.Responses["401"];
        AssertHeaderSummary(response422);
        response422.Description.Should().Be("<strong>AuthFailure</strong><p>Not Supported</p>");

        //Base Class Level
        var response500 = operation.Responses["500"];
        AssertHeaderSummary(response500);
        response500.Description.Should().Be("<strong>Failure</strong><p>Server Error</p>");

        static void AssertHeaderSummary(OpenApiResponse response2)
        {
            response2.Headers.Should().ContainKey("Header1").And.Subject.Should().NotBeNull();
            response2.Headers["Header1"].Description.Should().Be("Header1 summary");
            response2.Headers["Header1"].Schema.Should().BeEquivalentTo(
                new OpenApiSchema()
                {
                    Type = nameof(String)
                });
        }
    }

    private SwaggerCustomResponseFilter Subject() => new();
}

[SwaggerCustomResponse(statusCode: 500, type: typeof(StandardResponse), headerType: typeof(ResponseHeader), description: "<strong>Failure</strong><p>Server Error</p>")]
internal class TestControllerWithSwaggerAnnotationsBase
{
}

[SwaggerCustomResponse(statusCode: 401, type: typeof(Response), headerType: typeof(ResponseHeader), description: "<strong>AuthFailure</strong><p>Not Supported</p>")]
internal class TestControllerWithSwaggerAnnotations : TestControllerWithSwaggerAnnotationsBase
{
    public class Response : StandardResponse
    {
    }

    public class ResponseBody
    {
    }

    [SwaggerCustomResponse(statusCode: 200, type: typeof(Response), headerType: typeof(ResponseHeader), description: "<strong>Success</strong><p>Everything worked as expected</p>")]
    [SwaggerCustomResponse(statusCode: 404, type: typeof(Response), headerType: typeof(ResponseHeader), description: "<strong>Failure</strong><p>Resource not found</p>")]
    public void ActionWithSwaggerResponseAttributes()
    {
    }
}

internal class ResponseHeader
{
    [SwaggerSchema(Description = "Header1 summary")]
    public string? Header1 { get; set; }
}