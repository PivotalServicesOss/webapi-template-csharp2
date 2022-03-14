namespace PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;

public class GetWeatherForecastByZipCodeHandler : IRequestHandler<GetWeatherForecastByZipCodeRequest, GetWeatherForecastByZipCodeResponse>
{
    private readonly IRepository repository;

    public GetWeatherForecastByZipCodeHandler(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetWeatherForecastByZipCodeResponse> Handle(GetWeatherForecastByZipCodeRequest request, CancellationToken cancellationToken)
    {
        var forecasts = await repository.GetWeatherForecastByZipCodeAsync(request.ZipCode);

        var response = new GetWeatherForecastByZipCodeResponse
        {
            Status = Status.Success,
            WeatherForecasts = forecasts.Select(forecast =>
                        new GetWeatherForecastByZipCodeDetail
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

public class GetWeatherForecastByZipCodeRequest : IRequest, IRequest<GetWeatherForecastByZipCodeResponse>
{
    [SwaggerSchema(Description = "5 digit zipcode of the location", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("00000")]
    [Required]
    public string ZipCode { get; set; } = string.Empty;
}

public class GetWeatherForecastByZipCodeResponse : StandardResponse
{
    public IEnumerable<GetWeatherForecastByZipCodeDetail>? WeatherForecasts { get; set; }
}

public class GetWeatherForecastByZipCodeDetail
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

public class GetWeatherForecastByZipCodeValidator : AbstractValidator<GetWeatherForecastByZipCodeRequest>
{
    public GetWeatherForecastByZipCodeValidator()
    {
        RuleFor(p => p.ZipCode)
            .Matches(@"^\d{5}$")
            .WithMessage("{PropertyName} is not valid. Only 5 digit zipcodes are accepted");
    }
}