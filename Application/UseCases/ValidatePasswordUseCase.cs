using Application.Communication.Requests;
using Application.Communication.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

public class ValidatePasswordUseCase : IValidatePasswordUseCase
{
	private readonly ILogger<ValidatePasswordUseCase> _logger;

	public ValidatePasswordUseCase(ILogger<ValidatePasswordUseCase> logger)
	{
		_logger = logger;
	}

	public async Task<PasswordResponse> Execute(PasswordRequest request)
	{
		var validator = new ValidatePasswordValidator();
		var result = await validator.ValidateAsync(request.Password);

		if (!result.IsValid)
		{
			var errorMessages = result.Errors
				.Select(error => error.ErrorMessage).ToList();

			var concatenatedErrors = string.Join(" | ", errorMessages);

			_logger.LogWarning("password validation failed: {Errors}", concatenatedErrors);

			return new PasswordResponse
			{
				IsValid = false,
				Message = concatenatedErrors
			};
		}

		return new PasswordResponse
		{
			IsValid = true,
			Message = "valid password"
		};
	}
}
