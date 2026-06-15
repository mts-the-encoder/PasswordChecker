using Application.Communication.Requests;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PasswordController : BaseController
{
	[HttpPost("validate")]
	public async Task<IActionResult> Create([FromServices] IValidatePasswordUseCase useCase, [FromBody] PasswordRequest request)
	{
		var response = await useCase.Execute(request);

		if (!response.IsValid)
			return BadRequest(response);

		return Ok(response);
	}
}