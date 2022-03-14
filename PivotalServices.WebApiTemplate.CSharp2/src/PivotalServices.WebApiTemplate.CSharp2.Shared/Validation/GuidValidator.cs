namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Validation;

public class GuidValidator<T,TProperty> : PropertyValidator<T,TProperty>
{
    public override string Name => "GuidValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} must be a valid GUID, with format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
    }

    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        if(value is not string _value)
                return false;

        return Guid.TryParse(_value, out Guid _);
    }
}
