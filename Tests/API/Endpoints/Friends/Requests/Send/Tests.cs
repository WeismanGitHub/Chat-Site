using API.Endpoints.Friends.Requests.Send;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Endpoints.Friends.Requests.Send;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_Friend_Request() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = Fixture.UserID2
        });

        res.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Duplicated_Friend_Request() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = Fixture.UserID2
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

	[Fact]
	public async Task Already_Friends() {
		await DB.Update<User>()
			.Modify(u => u.FriendIDs, new List<string> { Fixture.UserID1 })
			.MatchID(Fixture.UserID2).ExecuteAsync();

        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = Fixture.UserID2
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Invalid_RecipientID() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Invalid_Account() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = ObjectId.GenerateNewId().ToString(),
            Message = "Let's be friends.",
            RecipientID = Fixture.UserID2
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Invalid_Recipient() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "Let's be friends.",
            RecipientID = ObjectId.GenerateNewId().ToString()
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Message_Too_Long() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = new string('*', 251),
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Message_Too_Short() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
            Message = "",
            RecipientID = "invalid id"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
