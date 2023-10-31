using API.Endpoints.Account.Signup;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.Account.Signup;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_User_Input() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = ValidAccount.Password
        });

        res.IsSuccessStatusCode.Should().BeTrue();
        res.Headers.Any(header => header.Key == "Set-Cookie").Should().BeTrue();

        var acc = await DB.Find<User>().Match(u => u.Email == ValidAccount.Email).ExecuteSingleAsync();
        acc.Should().NotBeNull();
    }

    [Fact, Priority(2)]
    public async Task Taken_Email() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.Email,
            Email = ValidAccount.Email,
            Password = ValidAccount.Password
        });

        res.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Empty_DisplayName() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "",
            Email = ValidAccount.Email,
            Password = ValidAccount.Password
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DisplayName_Too_Long() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = new string('*', 26),
            Email = ValidAccount.Email,
            Password = ValidAccount.Password
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Empty_Email() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = "",
            Password = ValidAccount.Password
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task EmptyPassword() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "",
            Email = ValidAccount.Email,
            Password = ValidAccount.Password
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Too_Long() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = "VP1" + new string('*', 68) // VP1 is to meet the other requirements.
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Too_Short() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = "VP1" + new string('*', 6) // VP1 is to meet the other requirements.
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Password_Missing_Digit() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = "InvalidPassword"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }    

    [Fact]
    public async Task Password_Missing_Uppercase() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = "invalidpassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Missing_Lowercase() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = ValidAccount.DisplayName,
            Email = ValidAccount.Email,
            Password = "INVALIDPASSWORD1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
