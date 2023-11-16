using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Friends.Get;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public string AccountID { get; set; }
	private List<User> _users { get; set; }

	protected override async Task SetupAsync() {
		_users = FakeData.GenerateUsers(15);

		var account = FakeData.GenerateUsers(1).First();
		account.FriendIDs = _users.Select(u => u.ID).ToList();
		AccountID = account.ID;

		await DB.InsertAsync(_users);
		await DB.InsertAsync(account);

		await Task.Delay(5000);

		await Client.POSTAsync<Signin.Endpoint, Signin.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(_users.Select(u => u.ID));
		await DB.DeleteAsync<User>(AccountID);
	}
}
