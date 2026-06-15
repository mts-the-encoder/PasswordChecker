namespace Application.Exceptions;

public class ValidationErrorsException(List<string> errorMessages) : Exception("one or more validation errors occurred")
{
	public List<string> ErrorMessages { get; } = errorMessages;
}