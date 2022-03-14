namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public static class StatusDescription
{
    public const string Error = "A server error eccured, please contact the application owner";
    public const string Timeout = "Sorry, timed out while processing the request, please try again or contact the application owner";
    public const string UnAuthorized = "Auth failure occured (Unauthorized), Not authorized to access the resource";
    public const string Forbidden = "Auth failure occured (Forbidden), Not authorized to access the resource";
}
