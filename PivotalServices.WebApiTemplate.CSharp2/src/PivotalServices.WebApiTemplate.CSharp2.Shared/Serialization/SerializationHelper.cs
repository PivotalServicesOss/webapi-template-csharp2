namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Serialization;

public class SerializationHelper
{
    public static JsonSerializerOptions GetSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true 
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}