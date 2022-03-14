namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;

/// <summary>
/// Enriches SwaggerGen with response headers in addition to body schema. 
/// </summary>
/// <remarks>
/// Used by <see cref="SwaggerCustomResponseFilter"/>.
/// This Attributes extends the behavior of <seealso cref="SwaggerResponseAttribute"/> to enable Swagger
/// documentation of response headers.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class SwaggerCustomResponseAttribute : SwaggerResponseAttribute
{
    public SwaggerCustomResponseAttribute(int statusCode, string? description = null, Type? type = null, Type? headerType = null, params string[] contentTypes) 
        : base(statusCode, description, type, contentTypes)
    {
        HeaderType = headerType;
    }

    /// <summary>
    /// Gets or sets the header type of the value returned by an action
    /// </summary>
    public Type? HeaderType { get; }
}