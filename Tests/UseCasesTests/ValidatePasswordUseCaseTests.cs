using Application.Communication.Requests;
using Application.UseCases;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UseCasesTests;

public class ValidatePasswordUseCaseTests
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
	public async Task Execute_PasswordExamples_ReturnsExpectedResponse(string password)
	{
		// Arrange
		var loggerMock = new Mock<ILogger<ValidatePasswordUseCase>>();
		var useCase = new ValidatePasswordUseCase(loggerMock.Object);
		var request = new PasswordRequest { Password = password };

		// Act
		var response = await useCase.Execute(request);

		// Assert
		if (password == "AbTp9!fok")
		{
			response.IsValid.Should().BeTrue();
			response.Message.Should().Be("valid password");
		}
		else
		{
			response.IsValid.Should().BeFalse();
			response.Message.Should().NotBeNullOrWhiteSpace();
		}
	}

	[Fact]
	public async Task Execute_ValidRandomPassword_ReturnsValid()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<ValidatePasswordUseCase>>();
		var useCase = new ValidatePasswordUseCase(loggerMock.Object);

		var password = "Ab1!Cdefg"; 
		var request = new PasswordRequest { Password = password };

		// Act
		var response = await useCase.Execute(request);

		// Assert
		response.IsValid.Should().BeTrue();
	}
}
