using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PivotalServices.WebApiTemplate.CSharp2.Modules.WeatherForecast;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PivotalServices.WebApiTemplate.CSharp2.Application.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    private readonly IMediator mediator;

    public WeatherForecastController(IMediator mediator)
    {
        this.mediator = mediator;
    }
   
    [SwaggerOperation(Summary = "Gets weather forecast for all locations")]
    [SwaggerCustomResponse(StatusCodes.Status200OK, "Successfully returns weather forecasts", 
        typeof(GetWeatherForecastResponse), typeof(StandardResponseHeader), MediaTypeNames.Application.Json)]
    [HttpGet("GetAll", Name = "GetWeatherForecast")]
    public async Task<ActionResult<GetWeatherForecastResponse>> GetAsync([SwaggerFromHeader] StandardRequestHeader header,
                                                                                           CancellationToken cancellationToken)
    {
        return new OkObjectResult(await mediator.Send<GetWeatherForecastResponse>(new GetWeatherForecastRequest(), cancellationToken));
    }

    [SwaggerOperation(Summary = "Gets weather forecast for given zipcode")]
    [SwaggerCustomResponse(StatusCodes.Status200OK, "Successfully returns weather forecasts", 
        typeof(GetWeatherForecastByZipCodeResponse), typeof(StandardResponseHeader), MediaTypeNames.Application.Json)]
    [HttpGet("GetByZipCode", Name = "GetWeatherForecastByZipCode")]
    public async Task<ActionResult<GetWeatherForecastByZipCodeResponse>> GetByZipCodeAsync([SwaggerFromHeader] StandardRequestHeader header, 
                                                                                           [FromQuery] GetWeatherForecastByZipCodeRequest request,
                                                                                           CancellationToken cancellationToken)
    {
        return new OkObjectResult(await mediator.Send<GetWeatherForecastByZipCodeResponse>(request, cancellationToken));
    }
}
