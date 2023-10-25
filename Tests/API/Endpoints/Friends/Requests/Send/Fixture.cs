using API.Database.Entities;

using FastEndpoints.Security;

using MongoDB.Entities;

namespace Tests.API.Endpoints.Friends.Requests.Send;

public class Fixture : TestFixture<Program> {
    public Fixture(IMessageSink sink) : base(sink) { }
    public User Token1 { get; set; }
    public User Token2 { get; set; }

    protected override async Task SetupAsync() {
        await DB.InsertAsync(new User() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
        });
        
        await DB.InsertAsync(new User() {
            DisplayName = ValidAccount.DisplayName,
            Email = "2@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
        });
    }

    protected override void ConfigureServices(IServiceCollection services) {
    }

    protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email);
    }
}
