using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Account.Signup;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }

	protected override Task SetupAsync() {
		return Task.CompletedTask;
	}

	protected override void ConfigureServices(IServiceCollection services) {
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email);
	}
}
