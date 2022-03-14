using PivotalServices.WebApiTemplate.CSharp2.Shared.Validation;

public class UtcDateTimeValidatorTests
{
    [Fact]
    public void Test_Validator_ValidatesIfTheGivenStringIsAValidDateTimeUtc()
    {
        //Arrange
        var validator = new UtcDateTimeValidatorStub();

        //Act and Assert
        validator.Validate(new UtcDateTimeTestModel { TestDateTimeUtc = "invalid datetime"}).IsValid.Should().BeFalse();
        validator.Validate(new UtcDateTimeTestModel { TestDateTimeUtc = "2999-12-31 00:00:00.000"}).IsValid.Should().BeFalse();
        validator.Validate(new UtcDateTimeTestModel { TestDateTimeUtc = "2999-12-31T00:00:00Z"}).IsValid.Should().BeTrue();
        validator.Validate(new UtcDateTimeTestModel { TestDateTimeUtc = "2999-12-31T00:00:00.000Z"}).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_DateTimeUtcValidatorMessage_ShouldBePresent()
    {
        //Arrange
        var validator = new UtcDateTimeValidatorStub();

        //Act
        var result = validator.Validate(new UtcDateTimeTestModel { TestDateTimeUtc = "invalid datetime"});

        //Assert
        result.Errors.Should().ContainSingle(v => v.ErrorMessage == "Test Date Time Utc must be a valid UTC datetime, with format yyyy-MM-ddTHH:mm:ss.tttZ");
    }
}

class UtcDateTimeValidatorStub : AbstractValidator<UtcDateTimeTestModel>
{
    public UtcDateTimeValidatorStub()
    {
        RuleFor(p => p.TestDateTimeUtc).IsValidUtcDateTime();
    }
}

record UtcDateTimeTestModel
{
    public string? TestDateTimeUtc { get; set; }
}
