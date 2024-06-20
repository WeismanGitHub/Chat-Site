using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Requests.Decline;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string UserID1 = ObjectId.GenerateNewId().ToString();

	public readonly string RequestID1 = ObjectId.GenerateNewId().ToString();
	public readonly string RequestID2 = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = UserID1,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await DB.InsertAsync(new List<FriendRequest>() {
			new () {
				ID = RequestID1,
				RecipientID = UserID1,
				RequesterID = ObjectId.GenerateNewId().ToString(),
			},
			new () {
				ID = RequestID2,
				RecipientID = ObjectId.GenerateNewId().ToString(),
				RequesterID = ObjectId.GenerateNewId().ToString()
			},
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(UserID1);
		await DB.DeleteAsync<FriendRequest>(new List<string>() { RequestID1, RequestID2 });
	}
}
