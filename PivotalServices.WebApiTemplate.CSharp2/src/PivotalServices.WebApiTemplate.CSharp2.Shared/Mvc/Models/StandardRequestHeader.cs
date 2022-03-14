namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardRequestHeader
{
    [SwaggerSchema(Description = "Unique ID (correlation) generated per request", Format = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx")]
    [SwaggerSchemaExample("00000000-0000-0000-0000-000000000000")]
    [Required]
    [FromHeader]
    public string? CorrelationId { get; set; }

    [SwaggerSchema(Description = "Current timestamp in UTC", Format = "yyyy-MM-ddTHH:mm:ss.FFFZ")]
    [SwaggerSchemaExample("2022-02-09T00:00:00.000Z")]
    [Required]
    [FromHeader]
    public string? RequestDateTimeUtc { get; set; }
}