namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardResponse
{
    [SwaggerSchema(Description = "The standard response status like success, failure, error, etc.")]
    public Status Status { get; set; } = Status.Success;
}