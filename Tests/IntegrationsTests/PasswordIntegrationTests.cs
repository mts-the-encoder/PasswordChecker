using System.Net;
using System.Net.Http.Json;
using Application.Communication.Requests;
using Application.Communication.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationsTests;

public class PasswordIntegrationTests(WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>>
{
	[Theory]
	[InlineData("", HttpStatusCode.BadRequest)]
	[InlineData("aa", HttpStatusCode.BadRequest)]
	[InlineData("ab", HttpStatusCode.BadRequest)]
	[InlineData("AAAbbbCc", HttpStatusCode.BadRequest)]
	[InlineData("AbTp9!foo", HttpStatusCode.BadRequest)]
	[InlineData("AbTp9!foA", HttpStatusCode.BadRequest)]
	[InlineData("AbTp9 fok", HttpStatusCode.BadRequest)]
	[InlineData("AbTp9!fok", HttpStatusCode.OK)]
	public async Task PostPasswordValidate_Examples_ReturnsExpectedStatus(string password, HttpStatusCode expected)
	{
		// Arrange
		var client = factory.CreateClient();
		var request = new PasswordRequest { Password = password };

		// Act
		var response = await client.PostAsJsonAsync("/api/password/validate", request);

		// MACETE: se a API der pau (500), isso aqui captura o log exato do servidor e te mostra no console do xUnit
		if (response.StatusCode == HttpStatusCode.InternalServerError)
		{
			var errorMessage = await response.Content.ReadAsStringAsync();
			throw new Exception($"A API quebrou internamente! Motivo: {errorMessage}");
		}

		// Assert - valida o status http
		response.StatusCode.Should().Be(expected);

		// Assert - valida o corpo da resposta
		var payload = await response.Content.ReadFromJsonAsync<PasswordResponse>();

		if (expected == HttpStatusCode.OK)
		{
			payload.Should().NotBeNull();
			payload!.IsValid.Should().BeTrue();
		}
		else
		{
			payload.Should().NotBeNull();
			payload!.IsValid.Should().BeFalse();
			payload.Message.Should().NotBeNullOrEmpty();
		}
	}
}