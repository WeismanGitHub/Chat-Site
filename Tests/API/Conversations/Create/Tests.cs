using API.Endpoints.Conversations.Create;
using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Conversations.Create;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact, Priority(1)]
	public async Task Successful_Request() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new() {
			ConversationName = "test"
		});

		rsp.IsSuccessStatusCode.Should().BeTrue();

		var account = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteFirstAsync();
		account.Should().NotBeNull();

		account!.ConversationIDs.Count.Should().Be(1);
		account.ConversationIDs.First().Should().Be(res.ConversationID);

		var convo = await DB.Find<Conversation>().MatchID(res.ConversationID).ExecuteFirstAsync();
		convo.Should().NotBeNull();

		convo!.MemberIDs.Count.Should().Be(1);
		convo!.MemberIDs.First().Should().Be(account.ID);
		convo!.Name.Should().Be("test");
	}
}
