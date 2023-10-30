using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Account;

public class SignupFixture : TestFixture<Program> {
    public SignupFixture(IMessageSink sink) : base(sink) { }

    protected override Task SetupAsync() {
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(IServiceCollection services) {
    }

    protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email);
    }
}
