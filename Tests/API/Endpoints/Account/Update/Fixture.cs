using APISignin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.Account.Update;

public class Fixture : TestFixture<Program> {
    public Fixture(IMessageSink sink) : base(sink) { }

	public string AccountID = ObjectId.GenerateNewId().ToString();

    protected override async Task SetupAsync() {
		await DB.InsertAsync(new User() {
			ID = AccountID,
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		await Client.POSTAsync<APISignin.Endpoint, APISignin.Request>(new APISignin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email);
    }
}
