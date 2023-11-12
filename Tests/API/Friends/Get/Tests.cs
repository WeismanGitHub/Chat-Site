using API.Endpoints.Friends.Get;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Friends.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Valid() {
		var (res, rsp) = await Fixture.Client.GETAsync<Endpoint, Request, List<User>>(
			new() { AccountID = Fixture.AccountID }
		);

		//res.IsSuccessStatusCode.Should().BeTrue();
		res.StatusCode.Should().Be(HttpStatusCode.OK);

		var account = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteSingleAsync();
		var friends = await DB
			.Find<User>()
				.Match(u => account!.FriendIDs.Contains(u.ID))
				.Project(u => new() {
					DisplayName = u.DisplayName,
					ID = u.ID,
					CreatedAt = u.CreatedAt
				})
				.ExecuteAsync();

		rsp.All(f => friends!.Contains(f)).Should().BeTrue();
	}
}
