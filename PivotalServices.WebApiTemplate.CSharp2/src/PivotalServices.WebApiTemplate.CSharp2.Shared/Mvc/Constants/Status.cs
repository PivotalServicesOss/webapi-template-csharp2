namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Success,
    Failed,
    Timeout,
    Error,
    AuthFailure
}
