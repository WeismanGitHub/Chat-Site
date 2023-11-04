using API.Endpoints.Friends.Requests.Decline;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.Friends.Requests.Decline;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID1,
        });

        res.IsSuccessStatusCode.Should().BeTrue();

		var friendRequest = await DB.Find<FriendRequest>().MatchID(Fixture.RequestID1).ExecuteSingleAsync();

		friendRequest.Should().NotBeNull();
		friendRequest!.Status.Should().Be(Status.Declined);
    }

    [Fact, Priority(2)]
    public async Task Request_Not_Pending() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID1,
        });

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

	[Fact]
	public async Task Invalid_RequestID() {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = "invalidid"
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Nonexistant_Request() {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = ObjectId.GenerateNewId().ToString(),
		});

		res.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Not_Recipient() {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID2,
		});

		res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
	}
}
