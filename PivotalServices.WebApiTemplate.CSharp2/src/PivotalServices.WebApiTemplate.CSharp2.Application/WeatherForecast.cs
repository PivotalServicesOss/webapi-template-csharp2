using PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;
using Swashbuckle.AspNetCore.Annotations;

namespace PivotalServices.WebApiTemplate.CSharp2;

public class WeatherForecast
{
    /// <summary>
    /// Datetime for weather
    /// </summary>
    /// <example>12-11-2022</example>
    public DateTime Date { get; set; }

    [SwaggerSchema(Description = "Temperature in centigrade")]
    [SwaggerSchemaExample("40")]
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
