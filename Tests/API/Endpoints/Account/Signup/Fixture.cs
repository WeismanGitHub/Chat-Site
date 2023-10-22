using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Account.Signup;

public class Fixture : TestFixture<Program> {
    public List<string> userIds = new();
    public Fixture(IMessageSink sink) : base(sink) { }

    protected override Task SetupAsync() {
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(IServiceCollection services) {
    }

    protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(userIds.ToArray());
    }
}
