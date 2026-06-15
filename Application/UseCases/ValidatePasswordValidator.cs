using FluentValidation;

namespace Application.UseCases;

public class ValidatePasswordValidator : AbstractValidator<string>
{
	public ValidatePasswordValidator()
	{
		RuleFor(password => password)
			.NotEmpty().WithMessage("password cannot be empty")
			.Must(NotContainWhitespaces).WithMessage("password cannot contain whitespaces");

		When(password => !string.IsNullOrWhiteSpace(password) && NotContainWhitespaces(password), () =>
		{
			RuleFor(password => password)
				.MinimumLength(9).WithMessage("password must be at least 9 characters long")
				.Matches("[0-9]").WithMessage("password must contain at least 1 digit")
				.Matches("[a-z]").WithMessage("password must contain at least 1 lowercase letter")
				.Matches("[A-Z]").WithMessage("password must contain at least 1 uppercase letter")
				.Matches(@"[!@#$%^&*()\-+]").WithMessage("password must contain at least 1 special character")
				.Must(NotHaveRepeatedCharacters).WithMessage("password cannot contain repeated characters");
		});
	}

	private bool NotContainWhitespaces(string password)
	{
		if (string.IsNullOrEmpty(password)) return false;
		return !password.Any(char.IsWhiteSpace);
	}

	private bool NotHaveRepeatedCharacters(string password)
	{
		if (string.IsNullOrEmpty(password)) return false;
		return password.Distinct().Count() == password.Length;
	}
}