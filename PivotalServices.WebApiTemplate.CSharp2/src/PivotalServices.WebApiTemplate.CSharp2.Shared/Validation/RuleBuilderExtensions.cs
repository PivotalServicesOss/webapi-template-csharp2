namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Validation;

public static class RuleBuilderExtensions
{
    //This is a sample extension method for rule builder, so that you can add more if needed
    public static IRuleBuilderOptions<T, string?> IsValidUtcDateTime<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.SetValidator(new UtcDateTimeValidator<T, string?>());
    }

    public static IRuleBuilderOptions<T, string?> IsValidGuid<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.SetValidator(new GuidValidator<T, string?>());
    }
}
