namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardResponseHeader
{
    [SwaggerSchema(Description = "CorrelationId header from the current request")]
    public string? CorrelationId { get; set; }

    [SwaggerSchema(Description = "Response originated timestamp in UTC, format yyyy-MM-ddTHH:mm:ss.FFFZ")]
    public string? ResponseDateTimeUtc { get; set; }
}