using API.Endpoints.Account.Signup;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Account.Signup;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_User_Input() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.IsSuccessStatusCode.Should().BeTrue();
        res.Headers.Any(header => header.Key == "Set-Cookie").Should().BeTrue();

        Fixture.AccountEmail = "valid@email.com";

        var acc = await DB.Find<User>().Match(u => u.Email == "valid@email.com").ExecuteSingleAsync();
        acc.Should().NotBeNull();
    }

    [Fact]
    public async Task Empty_DisplayName() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "",
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Too_Long_DisplayName() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = new string('*', 51),
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Null_DisplayName() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Empty_Email() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Null_Email() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
