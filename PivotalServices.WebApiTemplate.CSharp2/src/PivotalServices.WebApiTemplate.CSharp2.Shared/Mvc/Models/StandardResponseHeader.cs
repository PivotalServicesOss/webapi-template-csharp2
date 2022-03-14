namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardResponseHeader
{
    [SwaggerSchema(Description = "Unique GUID generated while responding to a request")]
    public string? ResponseId { get; set; }

    [SwaggerSchema(Description = "Response originated timestamp in UTC, format yyyy-MM-ddTHH:mm:ss.FFFZ")]
    public string? ResponseDateTimeUtc { get; set; }
}