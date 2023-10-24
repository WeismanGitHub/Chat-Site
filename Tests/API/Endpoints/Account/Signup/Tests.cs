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
    public async Task DisplayName_Too_Long() {
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

    [Fact]
    public async Task EmptyPassword() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "",
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Null_Password() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Too_Long() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "VP1" + new string('*', 68) // VP1 is to meet the other requirements.
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Too_Short() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "VP1" + new string('*', 6) // VP1 is to meet the other requirements.
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Password_Missing_Digit() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "InvalidPassword"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }    

    [Fact]
    public async Task Password_Missing_Uppercase() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "invalidpassword1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Password_Missing_Lowercase() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "INVALIDPASSWORD1"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
