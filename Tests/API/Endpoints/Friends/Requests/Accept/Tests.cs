using API.Endpoints.Friends.Requests.Accept;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.Friends.Requests.Accept;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_Friend_Request() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID1,
        });

		var user1 = await DB.Find<User>().MatchID(Fixture.UserID1).ExecuteSingleAsync();
		var user2 = await DB.Find<User>().MatchID(Fixture.UserID2).ExecuteSingleAsync();
		var friendRequest = await DB.Find<FriendRequest>().MatchID(Fixture.RequestID1).ExecuteSingleAsync();

        res.IsSuccessStatusCode.Should().BeTrue();

		user1.Should().NotBeNull();
		user2.Should().NotBeNull();
		friendRequest.Should().NotBeNull();

		user1!.FriendIDs.Contains(Fixture.UserID2).Should().BeTrue();
		user2!.FriendIDs.Contains(Fixture.UserID1).Should().BeTrue();
		friendRequest!.Status.Should().Be(Status.Accepted);
    }

    [Fact, Priority(2)]
    public async Task Request_Not_Pending() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID1,
        });

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

	[Fact, Priority(2)]
	public async Task Already_Friends() {
		var newRequestID = ObjectId.GenerateNewId().ToString();

		await DB.InsertAsync(new FriendRequest() {
			ID = newRequestID,
			RecipientID = Fixture.UserID1,
			RequesterID = Fixture.UserID2,
		});

		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = newRequestID
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await DB.DeleteAsync<FriendRequest>(newRequestID);
	}

	[Fact, Priority(3)]
	public async Task Recipient_Has_Max_Friends() {
		await DB.Update<User>()
			.MatchID(Fixture.UserID1).Modify(u => u.FriendIDs, Enumerable.Repeat("friendID", 100).ToList())
			.ExecuteAsync();

		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID4
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await DB.Update<User>()
			.MatchID(Fixture.UserID1).Modify(u => u.FriendIDs, new List<string>())
			.ExecuteAsync();
	}

	[Fact, Priority(3)]
	public async Task Requester_Has_Max_Friends() {
		await DB.Update<User>()
			.MatchID(Fixture.UserID3).Modify(u => u.FriendIDs, Enumerable.Repeat("friendID", 100).ToList())
			.ExecuteAsync();

		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID4
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
	public async Task Nonexistant_Requester() {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID3
		});

		res.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
