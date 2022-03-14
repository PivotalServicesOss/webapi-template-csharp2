public class SerializationHelperTests
{
    [Fact]
    public void GetSerializerOptions_ShouldReturn()
    {
        var expected = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        expected.Converters.Add(new JsonStringEnumConverter());

        SerializationHelper.GetSerializerOptions().Should().BeEquivalentTo(expected);
    }
}