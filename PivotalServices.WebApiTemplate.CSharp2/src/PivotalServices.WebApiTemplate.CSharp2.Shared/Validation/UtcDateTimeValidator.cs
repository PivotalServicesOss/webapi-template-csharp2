namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Validation;

public class UtcDateTimeValidator<T,TProperty> : PropertyValidator<T,TProperty>
{
    public override string Name => "UtcDateTimeValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} must be a valid UTC datetime, with format yyyy-MM-ddTHH:mm:ss.tttZ";
    }

    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        if (value is not string _value)
            return false;

        return DateTime.TryParse(_value, null,
            DateTimeStyles.AdjustToUniversal, // this is important
            out DateTime date) && date.Kind == DateTimeKind.Utc;
    }
}
