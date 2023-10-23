using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Account.Signup;

public class Fixture : TestFixture<Program> {
    public Fixture(IMessageSink sink) : base(sink) { }

    public List<string> userEmails = new();
    public string Token { get; set; } = "";

    protected override Task SetupAsync() {
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(IServiceCollection services) {
    }

    protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(u => userEmails.Contains(u.Email));
    }
}
