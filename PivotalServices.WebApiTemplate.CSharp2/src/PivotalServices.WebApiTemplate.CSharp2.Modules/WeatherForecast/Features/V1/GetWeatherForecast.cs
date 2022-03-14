namespace PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;

public class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastRequest, GetWeatherForecastResponse>
{
    private readonly IRepository repository;

    public GetWeatherForecastHandler(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetWeatherForecastResponse> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var forecasts = await repository.GetWeatherForecastsAsync();

        var response = new GetWeatherForecastResponse
        {
            WeatherForecasts = forecasts.Select(forecast =>
                        new GetWeatherForecastDetail
                        {
                            Date = forecast.Date.ToString("MM-dd-yyyy"),
                            TemperatureC = forecast.TemperatureC,
                            Summary = forecast.Summary,
                            ZipCode = forecast.ZipCode
                        })
        };

        return response;
    }
}

public class GetWeatherForecastRequest : IRequest, IRequest<GetWeatherForecastResponse>
{
   
}

public class GetWeatherForecastResponse : StandardResponse
{
    public IEnumerable<GetWeatherForecastDetail>? WeatherForecasts { get; set; }
}

public class GetWeatherForecastDetail
{
    [SwaggerSchema(Description = "Date of forecast")]
    [SwaggerSchemaExample("02-09-2022")]
    public string? Date { get; set; }

    [SwaggerSchema(Description = "Temperature in Celcius")]
    public int TemperatureC { get; set; }

    [SwaggerSchema(Description = "Temperature in Farenhit", ReadOnly = true)]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [SwaggerSchema(Description = "Summary of the weather forecast")]
    public string? Summary { get; set; }

    [SwaggerSchema(Description = "5 digit zipcode")]
    public string? ZipCode { get; set; }

}