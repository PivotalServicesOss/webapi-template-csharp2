namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Enum, AllowMultiple = false)]
public class SwaggerSchemaExampleAttribute : Attribute
{
    public SwaggerSchemaExampleAttribute(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
