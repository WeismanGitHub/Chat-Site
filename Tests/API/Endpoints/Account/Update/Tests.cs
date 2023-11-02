using API.Endpoints.Account.Update;
using API.Database.Entities;
using MongoDB.Entities;
using Microsoft.Identity.Client;

namespace Tests.Account.Update;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact, Priority(1)]
	public async Task Valid_Update() {
        var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				DisplayName = "New DisplayName",
				Email = "new" + ValidAccount.Email,
				Password = ValidAccount.Password + "new"
			},
			CurrentPassword = ValidAccount.Password
        });

		res.StatusCode.Should().Be(HttpStatusCode.NoContent);

		var acc = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteSingleAsync();
        acc.Should().NotBeNull();

		acc!.DisplayName.Should().Be("New DisplayName");
		acc!.Email.Should().Be("new" + ValidAccount.Email);
		BCrypt.Net.BCrypt.Verify(ValidAccount.Password + "new", acc!.PasswordHash).Should().BeTrue();
    }

	[Fact]
	public async Task Invalid_CurrentPassword() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				DisplayName = "New DisplayName",
			},
			CurrentPassword = "invalid"
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task No_Changes() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Taken_Email() {
		await DB.InsertAsync(new User() {
			DisplayName = ValidAccount.DisplayName,
			Email = "taken" + ValidAccount.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
		});

		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Email = "taken" + ValidAccount.Email,
			},
			CurrentPassword = ValidAccount.Password
		});

		await DB.DeleteAsync<User>((u) => u.Email == "taken" + ValidAccount.Email);
		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Empty_Email() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Email = "",
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Empty_DisplayName() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				DisplayName = "",
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task DisplayName_Too_Long() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				DisplayName = new string('*', User.MaxNameLength + 1),
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	//[Fact]
	//public async Task EmptyPassword() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = "",
	//        Email = ValidAccount.Email,
	//        Password = ValidAccount.Password
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Null_Password() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        Email = ValidAccount.Email,
	//        Password = ValidAccount.Password
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Password_Too_Long() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = ValidAccount.DisplayName,
	//        Email = ValidAccount.Email,
	//        Password = "VP1" + new string('*', 68) // VP1 is to meet the other requirements.
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Password_Too_Short() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = ValidAccount.DisplayName,
	//        Email = ValidAccount.Email,
	//        Password = "VP1" + new string('*', 6) // VP1 is to meet the other requirements.
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Password_Missing_Digit() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = ValidAccount.DisplayName,
	//        Email = ValidAccount.Email,
	//        Password = "InvalidPassword"
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}    

	//[Fact]
	//public async Task Password_Missing_Uppercase() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = ValidAccount.DisplayName,
	//        Email = ValidAccount.Email,
	//        Password = "invalidpassword1"
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Password_Missing_Lowercase() {
	//    var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//        DisplayName = ValidAccount.DisplayName,
	//        Email = ValidAccount.Email,
	//        Password = "INVALIDPASSWORD1"
	//    });

	//    res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}
}
