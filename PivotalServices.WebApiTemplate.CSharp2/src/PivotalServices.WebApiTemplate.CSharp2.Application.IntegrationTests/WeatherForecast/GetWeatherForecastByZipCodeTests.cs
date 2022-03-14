using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Serialization;
using Xunit;
using Xunit.Abstractions;

[Collection(nameof(GetWeatherForecastByZipCodeTests))]
public class GetWeatherForecastByZipCodeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper outputHelper;
    private readonly WebApplicationFactory<Program> factory;

    public GetWeatherForecastByZipCodeTests(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
        this.factory = factory.WithTestConfiguration(outputHelper);
    }

    [Fact]
    public async Task GivenAValidRequest_GetAllShouldReturn200AndGetWeatherForecastByZipCodeResponseWithStatusSuccessAndNotNullWeatherForecasts()
    {
        var testClient = factory.CreateClient();

        testClient.DefaultRequestHeaders.Add("CorrelationId", "f0e1865e-cc95-475c-af8b-91d2c2c71777");
        testClient.DefaultRequestHeaders.Add("RequestDateTimeUtc", "2999-12-31T00:00:00.000Z");

        var zipCode = "12345";

        var response = await testClient.GetAsync($"/v1/WeatherForecast/GetByZipCode?ZipCode={zipCode}");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var bodyContent = await response.Content.ReadAsStringAsync();
        //outputHelper.WriteLine(bodyContent);

        var body = JsonSerializer.Deserialize<GetWeatherForecastByZipCodeResponse>(bodyContent, SerializationHelper.GetSerializerOptions());
        body?.Status.Should().Be(Status.Success);
        body?.WeatherForecasts.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenAnInValidRequest_GetAllShouldReturn400AndStandardErrorResponseWithFailedStatusAppropriateStatusDetails()
    {
        var testClient = factory.CreateClient();

        testClient.DefaultRequestHeaders.Add("CorrelationId", "07f1ff17-aa1a-4a70-826f-d540d1e3dbb3");
        testClient.DefaultRequestHeaders.Add("RequestDateTimeUtc", "2999-12-31T00:00:00.000Z");

        var zipCode = "invalid zipcode";
        var response = await testClient.GetAsync($"/v1/WeatherForecast/GetByZipCode?ZipCode={zipCode}");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var bodyContent = await response.Content.ReadAsStringAsync();
        //outputHelper.WriteLine(bodyContent);

        var body = JsonSerializer.Deserialize<StandardErrorResponse>(bodyContent, SerializationHelper.GetSerializerOptions());
        body?.Status.Should().Be(Status.Failed);
        body?.StatusDetails.Should().NotBeNull();
        body?.StatusDetails.Should().HaveCountGreaterThan(0);
    }
}