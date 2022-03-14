using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;

public class RepositoryTests
{
    [Fact]
    public async Task GetWeatherForecastsAsync_ShouldReturn25Forecasts()
    {
        Repository repository = new();
        var forecast = await repository.GetWeatherForecastsAsync();
        forecast.Should().NotBeNull();
        forecast.Should().HaveCount(25);
    }

    [Fact]
    public async Task GetWeatherForecastsAsync_ShouldReturn5ForecastsForTheGivenZipCode()
    {
        Repository repository = new();
        var forecast = await repository.GetWeatherForecastByZipCodeAsync("12345");
        forecast.Should().NotBeNull();
        forecast.Should().HaveCount(5);
    }
}