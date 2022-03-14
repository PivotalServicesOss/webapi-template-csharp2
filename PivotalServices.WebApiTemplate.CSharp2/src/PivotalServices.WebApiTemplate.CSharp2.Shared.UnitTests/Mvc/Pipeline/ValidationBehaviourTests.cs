public class ValidationBehaviourTests
{
    public int InvokeCount { get; private set; }

    [Fact]
    public async Task Handle_ShouldThrowValidationExceptionOnValidatorErrors()
    {
        List<IValidator<Request>> validators = new();
        var behavior = new ValidationBehavior<Request, Response>(validators);

        var validator = new TestValidator();
        validators.Add(validator);

        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    behavior.Handle(new Request() { TestProperty = "" }, default, Next));

        InvokeCount.Should().Be(0);
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_ShouldCallNextMiddlewareIfNoErrors()
    {
        List<IValidator<Request>> validators = new();
        var behavior = new ValidationBehavior<Request, Response>(validators);

        var validator = new TestValidator();
        validators.Add(validator);

        await behavior.Handle(new Request() { TestProperty = "non empty value" }, default, Next);

        InvokeCount.Should().BeGreaterThan(0);
    }

    private Task<Response> Next()
    {
        InvokeCount++;
        return Task.FromResult(new Response());
    }
}



internal class TestValidator : AbstractValidator<Request>
{
    public TestValidator()
    {
        RuleFor(p => p.TestProperty)
            .NotEmpty()
            .WithMessage("Should not be empty");
    }
}

internal class Request : IRequest<Response>
{
    public string? TestProperty { get; set; }
}

internal class Response
{

}