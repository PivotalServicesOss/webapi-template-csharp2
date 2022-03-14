using Swashbuckle.AspNetCore.Annotations;

public class SwaggerCustomResponseAttributeTests
{
    [Fact]
    public void SwaggerCustomResponse_ShouldBeOfTypeProducesResponseTypeAttribute()
    {
        //Assert
        typeof(SwaggerCustomResponseAttribute).Should().BeDerivedFrom(typeof(SwaggerResponseAttribute));
    }

    [Fact]
    public void Ctor_GivenConstructorParameters_ThenSetTheCorrectProperties()
    {
        //Arrange
        var expectedType = typeof(FakeResponseType);
        var expectedStatusCode = StatusCodes.Status200OK;
        var expectedHeaderType = typeof(FakeHeaderType);
        var expectedDescription = "Success";
        var expectedMediaType = MediaTypeNames.Application.Json;


        var responseType = new SwaggerCustomResponseAttribute(expectedStatusCode, expectedDescription, expectedType, expectedHeaderType, expectedMediaType);

        //Assert
        responseType.Type.Should().Be(expectedType);
        responseType.StatusCode.Should().Be(expectedStatusCode);
        responseType.HeaderType.Should().Be(expectedHeaderType);
        responseType.Description.Should().Be(expectedDescription);
        responseType.ContentTypes.Should().Contain(expectedMediaType);
    }

    [Fact]
    public void SwaggerCustomResponse_ShouldHaveAttributeUsageAttribute_WithCorrectParameters()
    {
        //Assert
        var constraint = typeof(SwaggerCustomResponseAttribute)
            .Should()
            .BeDecoratedWith<AttributeUsageAttribute>(a => a.Inherited == true && a.AllowMultiple == true 
                            && a.ValidOn == (AttributeTargets.Class | AttributeTargets.Method), "missing required parameters");
    }

    internal class FakeResponseType
    {

    }

    internal class FakeHeaderType
    {

    }
}
