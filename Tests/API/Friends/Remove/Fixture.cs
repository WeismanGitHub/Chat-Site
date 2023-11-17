using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Remove;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public string AccountID {  get; set; }
	public string FriendID {  get; set; }

	protected override async Task SetupAsync() {
		var account = FakeData.GenerateUsers(1).First();
		var friend = FakeData.GenerateUsers(1).First();

		account.FriendIDs = new() { friend.ID };
		friend.FriendIDs = new() { account.ID };
		account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password);

		AccountID = account.ID;
		FriendID = friend.ID;

		await DB.InsertAsync(new List<User> { account, friend });

		await Client.POSTAsync<Signin.Endpoint, Signin.Request>(new Signin.Request() {
			Email = account.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(new List<string>() { FriendID, AccountID });
	}
}
