using PivotalServices.WebApiTemplate.CSharp2.Shared.Validation;

public class GuidValidatorTests
{
    [Fact]
    public void Test_Validator_ValidatesIfTheGivenStringIsAValidGuid()
    {
        //Arrange
        var validator = new GuidValidatorStub();

        //Act and Assert
        validator.Validate(new GuidTestModel { TestGuid = "invalid guid" }).IsValid.Should().BeFalse();
        validator.Validate(new GuidTestModel { TestGuid = new Guid().ToString() }).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_GuidValidatorMessage_ShouldBePresent()
    {
        //Arrange
        var validator = new GuidValidatorStub();

        //Act
        var result = validator.Validate(new GuidTestModel { TestGuid = "invalid guid" });

        //Arrange
        result.Errors.Should().ContainSingle(v => v.ErrorMessage == "Test Guid must be a valid GUID, with format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
    }
}

class GuidValidatorStub : AbstractValidator<GuidTestModel>
{
    public GuidValidatorStub()
    {
        RuleFor(p => p.TestGuid).IsValidGuid();
    }
}

record GuidTestModel
{
    public string? TestGuid { get; set; }
}
