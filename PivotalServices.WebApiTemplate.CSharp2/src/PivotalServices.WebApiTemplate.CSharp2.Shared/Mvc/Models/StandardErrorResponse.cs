namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardErrorResponse : StandardResponse
{
    [SwaggerSchema(Description = "List of status message details")]
    public IList<StatusDetail>? StatusDetails { get; init; }
}