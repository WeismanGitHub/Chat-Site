using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Account.Signin;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	protected override Task SetupAsync() {
		return DB.InsertAsync(new User() {
			DisplayName = ValidAccount.DisplayName,
			Email = ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});
	}

	protected override void ConfigureServices(IServiceCollection services) {
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email);
	}
}
