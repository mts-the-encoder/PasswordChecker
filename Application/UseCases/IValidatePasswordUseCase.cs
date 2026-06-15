using Application.Communication.Requests;
using Application.Communication.Responses;

namespace Application.UseCases;

public interface IValidatePasswordUseCase
{
	Task<PasswordResponse> Execute(PasswordRequest request);
}