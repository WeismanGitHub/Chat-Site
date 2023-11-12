using API.Endpoints.Account.Update;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Account.Update;

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
			NewData = new() { },
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

	[Fact]
	public async Task Empty_Password() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "",
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Password_Too_Long() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "VP1" + new string('*', User.MaxPasswordLength - 2) // VP1 is to meet the other requirements.
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Password_Too_Short() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "VP1" + new string('*', User.MinPasswordLength - 1) // VP1 is to meet the other requirements.
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Password_Missing_Digit() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "InvalidPassword"
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Password_Missing_Uppercase() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "invalidpassword1"
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Password_Missing_Lowercase() {
		var res = await Fixture.Client.PATCHAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			NewData = new() {
				Password = "INVALIDPASSWORD1"
			},
			CurrentPassword = ValidAccount.Password
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
