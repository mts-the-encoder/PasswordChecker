using Application.Communication.Requests;
using Application.UseCases;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Helpers;

namespace Tests.UseCasesTests;

public class ValidatePasswordUseCaseTests
{
	[Theory]
	[MemberData(nameof(PasswordFaker.GetUnitScenarios), MemberType = typeof(PasswordFaker))]
	public async Task Execute_PasswordExamples_ReturnsExpectedResponse(string password, bool expectedIsValid)
	{
		#region Arrange
		var loggerMock = new Mock<ILogger<ValidatePasswordUseCase>>();
		var useCase = new ValidatePasswordUseCase(loggerMock.Object);
		var request = new PasswordRequest { Password = password };
		#endregion

		#region Act
		var response = await useCase.Execute(request);
		#endregion

		#region Assert
		response.IsValid.Should().Be(expectedIsValid);

		if (!expectedIsValid)
			response.Message.Should().NotBeNullOrWhiteSpace();
		#endregion
	}
}