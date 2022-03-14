namespace PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;

public class Repository : IRepository
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly string[] ZipCodes = new[]
    {
        "12345", "23456", "34567", "45678", "56789"
    };

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync()
    {
        return await GetAllForecasts();
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastByZipCodeAsync(string zipCode)
    {
        return (await GetAllForecasts()).Where(f => f.ZipCode == zipCode);
    }

    private Task<IEnumerable<WeatherForecast>> GetAllForecasts()
    {
        var forecasts = new List<WeatherForecast>();

        foreach (var zipcode in ZipCodes)
        {
            foreach (var index in Enumerable.Range(1, 5))
            {
                forecasts.Add(new WeatherForecast
                {
                    ZipCode = zipcode,
                    Date = DateTimeOffset.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }
        }

        return Task.FromResult(forecasts.AsEnumerable());
    }
}

public interface IRepository
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync();
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastByZipCodeAsync(string zipCode);
}