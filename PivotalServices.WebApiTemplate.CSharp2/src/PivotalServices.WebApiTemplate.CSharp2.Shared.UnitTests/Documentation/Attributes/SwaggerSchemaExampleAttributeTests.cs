public class SwaggerSchemaExampleAttributeTests
{
    [Fact]
    public void SwaggerSchemaExampleAttribute_ShouldBeOfTypeProducesResponseTypeAttribute()
    {
        //Assert
        typeof(SwaggerSchemaExampleAttribute).Should().BeDerivedFrom(typeof(Attribute));
    }

    [Fact]
    public void Ctor_GivenConstructorParameters_ThenSetTheCorrectProperties()
    {
        //Arrange
        var expectedValue = "some example";

        var responseType = new SwaggerSchemaExampleAttribute(expectedValue);

        //Assert
        responseType.Value.Should().Be(expectedValue);
    }
}
