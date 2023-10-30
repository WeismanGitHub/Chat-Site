using API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Friends.Requests.Send;

public class Fixture : TestFixture<Program> {
    public Fixture(IMessageSink sink) : base(sink) { }
    public readonly string UserID1 = "653c997d105a14b194a7e30b";
    public readonly string UserID2 = "653c997d105a14b194a7e30a";

    protected override async Task SetupAsync() {
        await DB.InsertAsync(new User() {
            ID = UserID1,
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
        });
        
        await DB.InsertAsync(new User() {
            ID = UserID2,
            DisplayName = ValidAccount.DisplayName,
            Email = "2@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
        });
    }

    protected override void ConfigureServices(IServiceCollection services) {
    }

    protected override async Task TearDownAsync() {
        await DB.DeleteAsync<User>(u => u.Email == ValidAccount.Email || u.Email == "2@email.com");
        await DB.DeleteAsync<FriendRequest>(u => u.RequesterID == UserID1);
    }

    public Task SignClientIn(Request req) {
        return Client.POSTAsync<Endpoint, Request>(req);
    }
}
