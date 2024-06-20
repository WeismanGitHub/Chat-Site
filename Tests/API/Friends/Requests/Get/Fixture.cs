using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Requests.Get;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string AccountID = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(
			FakeData.GenerateFriendRequests(21).Select(fr => {
				fr.RecipientID = AccountID;
				return fr;
			})
		);
		
		await DB.InsertAsync(
			FakeData.GenerateFriendRequests(10).Select(fr => {
				fr.RequesterID = AccountID;
				return fr;
			})
		);

		await DB.InsertAsync(new User() {
			ID = AccountID,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DropCollectionAsync<FriendRequest>();
		await DB.DeleteAsync<User>(AccountID);
	}
}
