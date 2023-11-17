using API.Endpoints.Friends.Remove;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Remove;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact, Priority(1)]
	public async Task Valid () {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			FriendID = Fixture.FriendID
		});

		res.IsSuccessStatusCode.Should().BeTrue();

		var account = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteSingleAsync();
		var friend = await DB.Find<User>().MatchID(Fixture.FriendID).ExecuteSingleAsync();

		account!.FriendIDs.Count.Should().Be(0);
		friend!.FriendIDs.Count.Should().Be(0);
	}

	[Fact]
	public async Task Removing_Nonexistant_Friend() {
		var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			FriendID = ObjectId.GenerateNewId().ToString(),
		});

		res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
