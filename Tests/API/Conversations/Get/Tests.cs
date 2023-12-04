using API.Endpoints.Conversations.Get;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Conversations.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Valid_Request() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, List<ResConvo>>(new());

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.Should().NotBeNull();

		var convos = await DB
			.Find<Conversation>()
			.Match(convo => res.Select(c => c.ID).Contains(convo.ID))
			.ExecuteAsync();

		convos.Count.Should().Be(res.Count);

		foreach (var convo in convos) {
			convo.MemberIDs.Contains(Fixture.AccountID).Should().BeTrue();
		}

		await DB.Update<User>().MatchID(Fixture.AccountID).Modify(u => u.ConversationIDs, new() { }).ExecuteAsync();
	}

	[Fact]
	public async Task No_Convos() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, List<ResConvo>>(new());

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.Count.Should().Be(0);
	}
}
