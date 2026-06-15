using Bogus;
using System.Net;

namespace Tests.Helpers;

public static class PasswordFaker
{
	private const string Lower = "abcdefghijklmnopqrstuvwxyz";
	private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private const string Digits = "0123456789";
	private const string Specials = "!@#$%^&*()-+";

	private static readonly List<(string Password, bool IsValid)> FixedExamples = new()
	{
		("", false),
		("aa", false),
		("ab", false),
		("AAAbbbCc", false),
		("AbTp9!foo", false),
		("AbTp9!foA", false),
		("AbTp9 fok", false),
		("AbTp9!fok", true)
	};

	public static IEnumerable<object[]> GetUnitScenarios()
	{
		foreach (var example in FixedExamples)
			yield return new object[] { example.Password, example.IsValid };

		foreach (var (password, isValid) in GenerateDynamicScenarios())
			yield return new object[] { password, isValid };
	}

	public static IEnumerable<object[]> GetIntegrationScenarios()
	{
		foreach (var example in FixedExamples)
			yield return new object[] { example.Password, example.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest };

		foreach (var (password, isValid) in GenerateDynamicScenarios())
			yield return new object[] { password, isValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest };
	}

	private static IEnumerable<(string Password, bool IsValid)> GenerateDynamicScenarios()
	{
		var faker = new Faker();

		for (int i = 0; i < 50; i++)
			yield return (GenerateValidPassword(faker), true);

		for (int i = 0; i < 50; i++)
			yield return (GenerateInvalidPassword(faker), false);
	}

	private static string GenerateValidPassword(Faker faker)
	{
		var passwordChars = new HashSet<char>();

		passwordChars.Add(faker.PickRandom(Lower.ToCharArray()));
		passwordChars.Add(faker.PickRandom(Upper.ToCharArray()));
		passwordChars.Add(faker.PickRandom(Digits.ToCharArray()));
		passwordChars.Add(faker.PickRandom(Specials.ToCharArray()));

		var allAllowed = (Lower + Upper + Digits + Specials).ToCharArray();
		var targetLength = faker.Random.Int(9, 12);

		while (passwordChars.Count < targetLength)
			passwordChars.Add(faker.PickRandom(allAllowed));

		return new string(faker.Random.Shuffle(passwordChars).ToArray());
	}

	private static string GenerateInvalidPassword(Faker faker)
	{
		var ruleToBreak = faker.Random.Int(1, 6);

		return ruleToBreak switch
		{
			1 => faker.Random.String2(faker.Random.Int(5, 8), Lower + Upper + Digits + Specials),
			2 => faker.Random.String2(10, Lower + Digits + Specials),
			3 => faker.Random.String2(10, Upper + Digits + Specials),
			4 => faker.Random.String2(10, Lower + Upper + Specials),
			5 => faker.Random.String2(10, Lower + Upper + Digits),
			_ => ForceRepeatCharacter(faker)
		};
	}

	private static string ForceRepeatCharacter(Faker faker)
	{
		var baseValid = GenerateValidPassword(faker);
		var charToRepeat = faker.PickRandom(baseValid.ToCharArray());
		return baseValid + charToRepeat;
	}
}