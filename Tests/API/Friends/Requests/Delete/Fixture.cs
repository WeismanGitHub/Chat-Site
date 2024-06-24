using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Requests.Delete;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string AccountID = ObjectId.GenerateNewId().ToString();
	public readonly string Request1ID = ObjectId.GenerateNewId().ToString();
	public readonly string Request2ID = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new List<FriendRequest>() {
			new () {
				ID = Request1ID,
				RecipientID = ObjectId.GenerateNewId().ToString(),
				RequesterID = AccountID
			},
			new () {
				ID = Request2ID,
				RecipientID = ObjectId.GenerateNewId().ToString(),
				RequesterID = ObjectId.GenerateNewId().ToString()
			}
		});

		await DB.InsertAsync(new User() {
			ID = AccountID,
			Name = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<FriendRequest>(new List<string>() { Request1ID, Request2ID });
		await DB.DeleteAsync<User>(AccountID);
	}
}
