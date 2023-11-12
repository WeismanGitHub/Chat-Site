using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using Tests.API;

namespace Tests.API.Friends.Get;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public string AccountID { get; set; }
	private List<User> _users { get; set; }

	protected override async Task SetupAsync() {
		_users = FakeData.GenerateUsers(15);

		var account = _users.First();
		account.FriendIDs = _users.Skip(1).Select(u => u.ID).ToList();
		AccountID = account.ID;

		_users[0] = account;
		await DB.InsertAsync(_users);

		await Client.POSTAsync<Signin.Endpoint, Signin.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(_users.Select(u => u.ID));
	}
}
