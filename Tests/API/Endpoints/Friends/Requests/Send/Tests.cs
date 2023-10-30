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

    [Fact]
    public async Task Invalid_RecipientID() {
        await Fixture.SignClientIn(new Signin.Request() {
            Email = ValidAccount.Email,
            Password = ValidAccount.Password,
        });

        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Message_Too_Long() {
        await Fixture.SignClientIn(new Signin.Request() {
            Email = ValidAccount.Email,
            Password = ValidAccount.Password,
        });

        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = new string('*', 251),
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Message_Too_Short() {
        await Fixture.SignClientIn(new Signin.Request() {
            Email = ValidAccount.Email,
            Password = ValidAccount.Password,
        });

        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "",
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
