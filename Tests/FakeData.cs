using API.Database.Entities;
using MongoDB.Bson;
using Bogus;

namespace Tests;

internal static class FakeData {
	private static Faker<User> UserFaker { get; set; }
	static FakeData() {
		Randomizer.Seed = new Random(12345);
		Faker.DefaultStrictMode = true;

		UserFaker = new Faker<User>()
			.RuleFor(u => u.ID, f => ObjectId.GenerateNewId().ToString())
			.RuleFor(u => u.DisplayName, f => f.Name.FirstName())
			.RuleFor(u => u.Email, f => f.Internet.Email())
			.RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
			.RuleFor(u => u.Conversations, f => null)
			.RuleFor(u => u.FriendIDs, f => new List<string>())
			.RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password(length: 15, prefix: "Pw1")));

	}

	public static List<User> GenerateUsers(int amount) {
		return UserFaker.GenerateForever().Take(amount).ToList();
	}
}
