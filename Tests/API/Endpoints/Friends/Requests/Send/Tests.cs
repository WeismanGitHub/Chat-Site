using Signin = API.Endpoints.Account.Signin;
using API.Endpoints.Friends.Requests.Send;

namespace Tests.API.Endpoints.Friends.Requests.Send;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_Friend_Request() {
        await Fixture.SignClientIn(new Signin.Request() {
            Email = ValidAccount.Email,
            Password = ValidAccount.Password,
        });

        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = Fixture.UserID2
        });

        res.IsSuccessStatusCode.Should().BeTrue();
    }
}
