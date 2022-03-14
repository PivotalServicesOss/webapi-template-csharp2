namespace PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;

public class StandardRequestHeaderValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var standardHeaderValue = context.ActionArguments.Values
            .SingleOrDefault(v => v is StandardRequestHeader);

        if (standardHeaderValue == null)
            await next.Invoke();
        else
        {
            var validationResult = await new Validator().ValidateAsync((StandardRequestHeader)standardHeaderValue);

            if (validationResult.IsValid)
                await next.Invoke();
            else
                throw new FluentValidation.ValidationException(validationResult.Errors);
        }
    }

    public class Validator : AbstractValidator<StandardRequestHeader>
    {
        public Validator()
        {
            RuleFor(p => p.CorrelationId)
                .IsValidGuid();

            RuleFor(p => p.RequestDateTimeUtc)
                .IsValidUtcDateTime();
        }
    }
}