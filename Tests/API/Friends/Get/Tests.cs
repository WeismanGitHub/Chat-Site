using API.Endpoints.Friends.Get;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Friends.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Valid() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, List<FriendResponse>>(new());

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.Should().NotBeNull();
		res.Count.Should().Be(14);

		var account = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteSingleAsync();
		var friendsDocuments = await DB
			.Find<User, FriendResponse>()
			.Match(u => account!.FriendIDs.Contains(u.ID))
			.Project(u => new() {
				ID = u.ID,
				DisplayName = u.DisplayName,
				CreatedAt = u.CreatedAt
			})
			.ExecuteAsync();

		foreach (var friend in res) {
			friendsDocuments.Contains(friend).Should().BeTrue();
		}
	}
}
