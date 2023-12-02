using SigninAPI = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Conversations.Create;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string AccountID = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = AccountID,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password),
		});

		await Client.POSTAsync<SigninAPI.Endpoint, SigninAPI.Request>(new SigninAPI.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		var account = await DB.Find<User>().MatchID(AccountID).ExecuteSingleAsync();

		await DB.DeleteAsync<Conversation>(account!.ConversationIDs.First());
		await account.DeleteAsync();
	}
}
