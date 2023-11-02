using API.Endpoints.Friends.Requests.Delete;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.Friends.Requests.Delete;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_Request() {
        var res = await Fixture.Client.DELETEAsync<Endpoint, Request>(new() {
            AccountID = Fixture.AccountID,
			RequestID = Fixture.Request1ID
        });

		res.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public async Task Invalid_RequestID() {
		var res = await Fixture.Client.DELETEAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			RequestID = "invalidid"
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
    public async Task Nonexistant_Request() {
		var res = await Fixture.Client.DELETEAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			RequestID = ObjectId.GenerateNewId().ToString(),
		});

		res.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Request_Not_From_Account() {
		var res = await Fixture.Client.DELETEAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			RequestID = Fixture.Request2ID,
		});

		res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
	}
}
