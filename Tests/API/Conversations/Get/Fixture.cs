using SigninAPI = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Conversations.Get;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string AccountID = ObjectId.GenerateNewId().ToString();
	private readonly string ConvoID = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = AccountID,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password),
		});

		await DB.InsertAsync(new List<ChatRoom>() {
			new ChatRoom() {
				ID = ConvoID,
				Name = "Convo 1",
				MemberIDs = new List<string>() { AccountID }
			}
		});

		await Client.POSTAsync<SigninAPI.Endpoint, SigninAPI.Request>(new SigninAPI.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(AccountID);
		await DB.DeleteAsync<ChatRoom>(ConvoID);
	}
}
