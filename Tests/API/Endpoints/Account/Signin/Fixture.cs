namespace Tests.API.Endpoints.Account.Signin;

public class Fixture : TestFixture<Program> {
    public Fixture(IMessageSink sink) : base(sink) { }

    protected override Task SetupAsync() {
        // place one-time setup for the test-class here
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(IServiceCollection services) {
        // do test service registration here
    }

    protected override Task TearDownAsync() {
        // do cleanups here
        return Task.CompletedTask;
    }
}
