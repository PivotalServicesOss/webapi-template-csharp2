namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StatusDetail
{
    [SwaggerSchema(Description = "Field or property name")]
    public string? FieldName { get; init; }

    [SwaggerSchema(Description = "Description of the corresponding status")]
    public string? Description { get; init; }

    [SwaggerSchema(Description = "A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807, expecially used for disgnostic purposes")]
    public ProblemDetails? ProblemDetails { get; init; }
}
