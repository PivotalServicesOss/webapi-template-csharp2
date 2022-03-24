public class DocumentationTests
{
    [Fact]
    public void StandardErrorResponse_ShouldBeDecoratedWithDocumentationAnnotation()
    {
        var typeUnderTest = typeof(StandardErrorResponse);

        var propertyName = "StatusDetails";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "List of status message details", $"{propertyName} required documentation");
    }

    [Fact]
    public void StandardRequestHeader_ShouldBeDecoratedWithDocumentationAnnotation()
    {
        var typeUnderTest = typeof(StandardRequestHeader);

        var propertyName = "CorrelationId";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Unique ID (correlation) generated per request" 
                        && s.Format == "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", $"{propertyName} required documentation");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaExampleAttribute>(s => 
                        s.Value == "00000000-0000-0000-0000-000000000000", $"{propertyName} required example");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<RequiredAttribute>($"{propertyName} required required");

        propertyName = "RequestDateTimeUtc";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Current timestamp in UTC" 
                        && s.Format == "yyyy-MM-ddTHH:mm:ss.FFFZ", $"{propertyName} required documentation");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaExampleAttribute>(s => 
                        s.Value == "2022-02-09T00:00:00.000Z", $"{propertyName} required example");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<RequiredAttribute>($"{propertyName} required required");
    }

    [Fact]
    public void StandardRequestHeaderProperties_ShouldBeDecoratedWithFromHeaderAttribute()
    {
        var typeUnderTest = typeof(StandardRequestHeader);

        var propertyName = "CorrelationId";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<FromHeaderAttribute>($"{propertyName} required fromheader attribute");

        propertyName = "RequestDateTimeUtc";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<FromHeaderAttribute>($"{propertyName} required fromheader attribute");
    }

    [Fact]
    public void StandardResponse_ShouldBeDecoratedWithDocumentationAnnotation()
    {
        var typeUnderTest = typeof(StandardResponse);

        var propertyName = "Status";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "The standard response status like success, failure, error, etc.", $"{propertyName} required documentation");
    }

    [Fact]
    public void StandardResponseHeader_ShouldBeDecoratedWithDocumentationAnnotation()
    {
        var typeUnderTest = typeof(StandardResponseHeader);

        var propertyName = "CorrelationId";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "CorrelationId header from the current request", $"{propertyName} missing required documentation");

        propertyName = "ResponseDateTimeUtc";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Response originated timestamp in UTC, format yyyy-MM-ddTHH:mm:ss.FFFZ", $"{propertyName} required documentation");
    }

    [Fact]
    public void StatusDetail_ShouldBeDecoratedWithDocumentationAnnotation()
    {
        var typeUnderTest = typeof(StatusDetail);

        var propertyName = "FieldName";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Field or property name", $"{propertyName} required documentation");

        propertyName = "Description";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Description of the corresponding status", $"{propertyName} required documentation");

        propertyName = "ProblemDetails";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807, expecially used for disgnostic purposes", $"{propertyName} required documentation");
    }
}
