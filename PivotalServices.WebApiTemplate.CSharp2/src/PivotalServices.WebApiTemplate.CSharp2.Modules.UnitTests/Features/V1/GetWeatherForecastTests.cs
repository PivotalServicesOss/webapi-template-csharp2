using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;
using Swashbuckle.AspNetCore.Annotations;

public class GetWeatherForecastTests
{
    [Fact]
    public async Task Handle_ShouldReturnFromRepository()
    {
        //Arrange            
        GetWeatherForecastRequest request = new ();

        var fromRepository = new List<WeatherForecast>
        {
            new WeatherForecast
            {
                ZipCode = "12345",
                Date = new DateTimeOffset(new DateTime(2022, 3, 10)),
                Summary = "Freeze",
                TemperatureC = -10
            }
        };

        var repository = new Mock<IRepository>();
        repository.Setup(x => x.GetWeatherForecastsAsync()).ReturnsAsync(fromRepository);

        //Act
        var handler = new GetWeatherForecastHandler(repository.Object);
        var response = await handler.Handle(request, default);

        //Assert
        var expected = new List<GetWeatherForecastDetail>
        {
            new GetWeatherForecastDetail
            {
                ZipCode = fromRepository[0].ZipCode,
                Date = fromRepository[0].Date.ToString("MM-dd-yyyy"),
                Summary = fromRepository[0].Summary,
                TemperatureC = fromRepository[0].TemperatureC,
            }
        };

        response.WeatherForecasts.Should().BeEquivalentTo(expected);
        response?.WeatherForecasts?.ToArray()[0].TemperatureF.Should().Be(32 + (int)(fromRepository[0].TemperatureC / 0.5556));
        response?.Status.Should().Be(Status.Success);

        repository.Verify(r => r.GetWeatherForecastsAsync(), Times.Once);
    }

    [Fact]
    public void ResponseProperties_ShouldBeDecoratedWithDocumentationAnnotations()
    {
        var typeUnderTest = typeof(GetWeatherForecastDetail);

        var propertyName = "Date";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Date of forecast", $"{propertyName} missing required documentation");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaExampleAttribute>(s => 
                        s.Value == "02-09-2022", $" {propertyName} missing example");

        propertyName = "TemperatureC";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Temperature in Celcius", $"{propertyName} missing required documentation");
        
        propertyName = "TemperatureF";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Temperature in Farenhit", $"{propertyName} missing required documentation");
        
        propertyName = "Summary";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Summary of the weather forecast", $"{propertyName} missing required documentation");
        
        propertyName = "ZipCode";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "5 digit zipcode", $"{propertyName} missing required documentation");
    }
}