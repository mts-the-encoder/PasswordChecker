using Application.UseCases;
using FluentAssertions;
using Tests.Helpers;

namespace Tests.UseCasesTests;

public class ValidatePasswordRequestValidatorTests
{
	[Theory]
	[MemberData(nameof(PasswordFaker.GetUnitScenarios), MemberType = typeof(PasswordFaker))]
	public void ValidatePassword_CenariosVariados_ReturnsExpected(string password, bool expectedIsValid)
	{
		#region Arrange
		var validator = new ValidatePasswordValidator();
		#endregion

		#region Act
		var result = validator.Validate(password);
		#endregion

		#region Assert
		result.IsValid.Should().Be(expectedIsValid);
		#endregion
	}
}