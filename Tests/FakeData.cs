using API.Database.Entities;
using MongoDB.Bson;
using Bogus;

namespace Tests;

internal static class FakeData {
	private static Faker<User> UserFaker { get; }
	private static Faker<FriendRequest> FriendRequestFaker { get; }
	static FakeData() {
		Randomizer.Seed = new Random(12345);
		Faker.DefaultStrictMode = true;

		UserFaker = new Faker<User>()
			.RuleFor(u => u.ID, _ => ObjectId.GenerateNewId().ToString())
			.RuleFor(u => u.DisplayName, f => f.Name.FirstName())
			.RuleFor(u => u.Email, f => f.Internet.Email())
			.RuleFor(u => u.CreatedAt, _ => DateTime.UtcNow)
			.RuleFor(u => u.ChatRoomIDs, f => new List<string>())
			.RuleFor(u => u.FriendIDs, _ => new List<string>())
			.RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password(length: 15, prefix: "Pw1")));

		FriendRequestFaker = new Faker<FriendRequest>()
			.RuleFor(fr => fr.ID, _ => ObjectId.GenerateNewId().ToString())
			.RuleFor(fr => fr.RequesterID, _ => ObjectId.GenerateNewId().ToString())
			.RuleFor(fr => fr.RecipientID, _ => ObjectId.GenerateNewId().ToString())
			.RuleFor(fr => fr.Message, f => f.Lorem.Sentence())
			.RuleFor(fr => fr.Message, f => f.Lorem.Sentence())
			.RuleFor(u => u.CreatedAt, _ => DateTime.UtcNow)
			.RuleFor(fr => fr.Status, _ => Status.Pending);
	}

	public static List<User> GenerateUsers(int amount) {
		return UserFaker.GenerateForever().Take(amount).ToList();
	}

	public static List<FriendRequest> GenerateFriendRequests(int amount) {
		return FriendRequestFaker.GenerateForever().Take(amount).ToList();
	}
}
