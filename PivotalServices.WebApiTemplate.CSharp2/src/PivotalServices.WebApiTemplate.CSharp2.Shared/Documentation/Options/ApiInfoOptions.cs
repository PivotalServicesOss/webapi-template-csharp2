namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

public class ApiInfoOptions
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeprecationMessage { get; set; } = string.Empty;
    public ApiContact Contact { get; set; } = new ApiContact();
    public ApiLicense License { get; set; } = new ApiLicense();
}
