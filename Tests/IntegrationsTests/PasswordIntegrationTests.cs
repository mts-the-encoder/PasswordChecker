using Application.Communication.Requests;
using Application.Communication.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Tests.Helpers;

namespace Tests.IntegrationsTests;

public class PasswordIntegrationTests(WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>>
{
	[Theory]
	[MemberData(nameof(PasswordFaker.GetIntegrationScenarios), MemberType = typeof(PasswordFaker))]
	public async Task PostPasswordValidate_Examples_ReturnsExpectedStatus(string password, HttpStatusCode expectedStatus)
	{
		#region Arrange
		var client = factory.CreateClient();
		var request = new PasswordRequest { Password = password };
		#endregion

		#region Act
		var response = await client.PostAsJsonAsync("/api/password/validate", request);
		#endregion

		#region Assert 
		if (response.StatusCode == HttpStatusCode.InternalServerError)
		{
			var errorMessage = await response.Content.ReadAsStringAsync();
			throw new Exception($"error: {errorMessage}");
		}

		response.StatusCode.Should().Be(expectedStatus);

		var payload = await response.Content.ReadFromJsonAsync<PasswordResponse>();

		if (expectedStatus == HttpStatusCode.OK)
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
		#endregion
	}
}