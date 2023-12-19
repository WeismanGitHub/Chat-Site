using API.Endpoints.Conversations.SingleConvo.Leave;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Conversations.SingleConvo.Leave;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact, Priority(1)]
	public async Task Empty_Convo() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.ConvoID
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.OK);
		rsp.IsSuccessStatusCode.Should().BeTrue();

		var convo = await DB
			.Find<Conversation>()
			.MatchID(Fixture.ConvoID)
			.ExecuteSingleAsync();

		var account = await DB
			.Find<User>()
			.MatchID(Fixture.AccountID)
			.ExecuteSingleAsync();

		convo.Should().BeNull();
		account!.ConversationIDs.Count().Should().Be(0);
	}

	[Fact]
	public async Task Nonexistant() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = ObjectId.GenerateNewId().ToString()
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact, Priority(1)]
	public async Task Convo_Has_Members() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.FullConvoID
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.OK);
		rsp.IsSuccessStatusCode.Should().BeTrue();

		var convo = await DB
			.Find<Conversation>()
			.MatchID(Fixture.ConvoID)
			.ExecuteSingleAsync();

		var account = await DB
			.Find<User>()
			.MatchID(Fixture.AccountID)
			.ExecuteSingleAsync();

		convo!.MemberIDs.Count.Should().Be(1);
		account!.ConversationIDs.Count().Should().Be(0);
	}
}
