public class SwaggerSchemaExampleFilterTests
{
    [Fact]
    public void Should_Output_Examples()
    {
        OpenApiSchema schema = new()
        {
            Example = new Microsoft.OpenApi.Any.OpenApiString(string.Empty)
        };

        var memberInfo = typeof(SampleRequest).GetProperty(nameof(SampleRequest.CorrelationId));

        SchemaFilterContext filterContext = new SchemaFilterContext(type: null, schemaGenerator: null, 
                                                schemaRepository: null, memberInfo, parameterInfo: null);

        Subject().Apply(schema, filterContext);

        schema.Example.Should().BeEquivalentTo(new Microsoft.OpenApi.Any.OpenApiString("2e60d1f8-9163-4d02-a36c-ae56e79a6215"));
    }

    private SwaggerSchemaExampleFilter Subject() => new();
}

internal class SampleRequest
{
    [SwaggerSchemaExample("2e60d1f8-9163-4d02-a36c-ae56e79a6215")]
    public string? CorrelationId{ get; set; }
}