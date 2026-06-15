using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application;


public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		AddUseCase(services);
	}

	private static void AddUseCase(IServiceCollection services)
	{
		services.AddScoped<IValidatePasswordUseCase, ValidatePasswordUseCase>();
	}
}
