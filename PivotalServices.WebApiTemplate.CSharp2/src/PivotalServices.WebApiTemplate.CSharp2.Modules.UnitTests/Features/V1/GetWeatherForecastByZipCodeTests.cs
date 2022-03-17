using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;
using Swashbuckle.AspNetCore.Annotations;

public class GetWeatherForecastByZipCodeByZipCodeTests
{
    [Fact]
    public async Task Handle_ShouldReturnFromRepository()
    {
        //Arrange            
        var zipCode = "12345";

        GetWeatherForecastByZipCodeRequest request = new () { ZipCode = zipCode };

        var fromRepository = new List<WeatherForecast>
        {
            new WeatherForecast
            {
                ZipCode = zipCode,
                Date = new DateTimeOffset(new DateTime(2022, 3, 10)),
                Summary = "Freeze",
                TemperatureC = -10
            },
            new WeatherForecast
            {
                ZipCode = zipCode,
                Date = new DateTimeOffset(new DateTime(2022, 3, 11)),
                Summary = "Hot",
                TemperatureC = 22
            }
        };

        var repository = new Mock<IRepository>();
        repository.Setup(x => x.GetWeatherForecastByZipCodeAsync(It.IsAny<string>())).ReturnsAsync(fromRepository);

        //Act
        var handler = new GetWeatherForecastByZipCodeHandler(repository.Object);
        var response = await handler.Handle(request, default);

        //Assert
        var expected = new List<GetWeatherForecastByZipCodeDetail>
        {
            new GetWeatherForecastByZipCodeDetail
            {
                ZipCode = fromRepository[0].ZipCode,
                Date = fromRepository[0].Date.ToString("MM-dd-yyyy"),
                Summary = fromRepository[0].Summary,
                TemperatureC = fromRepository[0].TemperatureC,
            },
            new GetWeatherForecastByZipCodeDetail
            {
                ZipCode = fromRepository[1].ZipCode,
                Date = fromRepository[1].Date.ToString("MM-dd-yyyy"),
                Summary = fromRepository[1].Summary,
                TemperatureC = fromRepository[1].TemperatureC,
            }
        };

        response.WeatherForecasts.Should().BeEquivalentTo(expected);
        response?.WeatherForecasts?.ToArray()[0].TemperatureF.Should().Be(32 + (int)(fromRepository[0].TemperatureC / 0.5556));
        response?.Status.Should().Be(Status.Success);

        repository.Verify(r => r.GetWeatherForecastByZipCodeAsync(It.Is<string>(z => z == zipCode)), Times.Once);
    }

    [Theory]
    [InlineData("12345", true, null)]
    [InlineData("invalid zipcode", false, "Zip Code is not valid. Only 5 digit zipcodes are accepted")]
    [InlineData("", false, "Zip Code is not valid. Only 5 digit zipcodes are accepted")]
    public async Task Validator_ShouldValidateRequest(string zipCode, bool isValid, string errorMessage)
    {
        GetWeatherForecastByZipCodeRequest request = new () { ZipCode = zipCode };
        var result = await new GetWeatherForecastByZipCodeValidator().ValidateAsync(request);

        result.IsValid.Should().Be(isValid);

        if(!isValid)
            result.Errors[0].ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void ResponseProperties_ShouldBeDecoratedWithDocumentationAnnotations()
    {
        var typeUnderTest = typeof(GetWeatherForecastByZipCodeDetail);

        var propertyName = "Date";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Date of forecast", $"{propertyName} required documentation");

        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaExampleAttribute>(s => 
                        s.Value == "02-09-2022", $" {propertyName} required example");

        propertyName = "TemperatureC";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Temperature in Celcius", $"{propertyName} required documentation");
        
        propertyName = "TemperatureF";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Temperature in Farenhit", $"{propertyName} required documentation");
        
        propertyName = "Summary";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "Summary of the weather forecast", $"{propertyName} required documentation");
        
        propertyName = "ZipCode";
        typeUnderTest.GetProperty(propertyName).Should().BeDecoratedWith<SwaggerSchemaAttribute>(s => 
                        s.Description == "5 digit zipcode", $"{propertyName} required documentation");
    }
}