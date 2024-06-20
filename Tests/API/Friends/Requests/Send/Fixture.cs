using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Requests.Send;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string UserID1 = ObjectId.GenerateNewId().ToString();
	public readonly string UserID2 = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = UserID1,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await DB.InsertAsync(new User() {
			ID = UserID2,
			DisplayName = ValidAccount.DisplayName,
			Email = "2@email.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email || u.Email == "2@email.com");
		await DB.DeleteAsync<FriendRequest>(u => u.RequesterID == UserID1);
	}
}
