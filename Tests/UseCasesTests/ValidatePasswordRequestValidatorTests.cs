using Application.UseCases;
using FluentAssertions;

namespace Tests.UseCasesTests;

public class ValidatePasswordRequestValidatorTests
{
	[Theory]
	[InlineData("")]
	[InlineData("aa")]
	[InlineData("ab")]
	[InlineData("AAAbbbCc")]
	[InlineData("AbTp9!foo")]
	[InlineData("AbTp9!foA")]
	[InlineData("AbTp9 fok")]
	[InlineData("AbTp9!fok")]
	public void ValidatePasswordExamples_DataDriven_ReturnsExpected(string password)
	{
		// Arrange
		var validator = new ValidatePasswordValidator();

		// Act
		var result = validator.Validate(password);

		// Assert
		if (password == "AbTp9!fok")
		{
			result.IsValid.Should().BeTrue();
		}
		else
		{
			result.IsValid.Should().BeFalse();
		}
	}

	[Fact]
	public void ValidatePassword_AllRulesSatisfied_IsValid()
	{
		// Arrange
		var validator = new ValidatePasswordValidator();
		var password = "AbTp9!fok";

		// Act
		var result = validator.Validate(password);

		// Assert
		result.IsValid.Should().BeTrue();
	}

	[Theory]
	[InlineData("AbTp9!fo")] 
	[InlineData("AbTp9!fo ")] 
	[InlineData("AbTp9!foA")] 
	[InlineData("abtp9!fok")] 
	public void ValidatePassword_InvalidExamples_AreInvalid(string password)
	{
		// Arrange
		var validator = new ValidatePasswordValidator();

		// Act
		var result = validator.Validate(password);

		// Assert
		result.IsValid.Should().BeFalse();
	}
}
