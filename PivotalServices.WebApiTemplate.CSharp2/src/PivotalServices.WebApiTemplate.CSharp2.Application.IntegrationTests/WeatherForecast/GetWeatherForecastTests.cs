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

[Collection(nameof(GetWeatherForecastTests))]
public class GetWeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper outputHelper;
    private readonly WebApplicationFactory<Program> factory;

    public GetWeatherForecastTests(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
        this.factory = factory.WithTestConfiguration(outputHelper);
    }

    [Fact]
    public async Task GivenAValidRequest_GetAllShouldReturn200AndGetWeatherForecastResponseWithStatusSuccessAndNotNullWeatherForecasts()
    {
        var testClient = factory.CreateClient();

        testClient.DefaultRequestHeaders.Add("CorrelationId", "6b4b4d08-dd41-4556-8b94-05b53664074c");
        testClient.DefaultRequestHeaders.Add("RequestDateTimeUtc", "2999-12-31T00:00:00.000Z");

        var response = await testClient.GetAsync("/v1/WeatherForecast/GetAll");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var bodyContent = await response.Content.ReadAsStringAsync();
        //outputHelper.WriteLine(bodyContent);

        var body = JsonSerializer.Deserialize<GetWeatherForecastResponse>(bodyContent, SerializationHelper.GetSerializerOptions());
        body?.Status.Should().Be(Status.Success);
        body?.WeatherForecasts.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenAnInValidRequest_GetAllShouldReturn400AndStandardErrorResponseWithFailedStatusAppropriateStatusDetails()
    {
        var testClient = factory.CreateClient();

        testClient.DefaultRequestHeaders.Add("CorrelationId", "invalid correlation id");
        testClient.DefaultRequestHeaders.Add("RequestDateTimeUtc", "2999-12-31T00:00:00.000Z");

        var response = await testClient.GetAsync("/v1/WeatherForecast/GetAll");

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