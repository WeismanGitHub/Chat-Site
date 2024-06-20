using SigninAPI = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Account.Get;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string AccountID = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = AccountID,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password),
			ChatRoomIDs = new List<string> { "sdfs", "sdfs" },
			FriendIDs = new List<string> { "sdfs", "sdfs" }
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new SigninAPI.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(AccountID);
	}
}
