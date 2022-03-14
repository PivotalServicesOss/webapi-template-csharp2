namespace PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;

public class WeatherForecast
{
    public DateTimeOffset Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
    public string? ZipCode { get; set; }
}