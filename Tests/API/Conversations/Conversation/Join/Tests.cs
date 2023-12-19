using API.Endpoints.Conversations.SingleConvo.Join;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Conversations.SingleConvo.Join;

[DefaultPriority(0)]
public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact, Priority(1)]
	public async Task Valid_Request() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.ConvoID
		});

		rsp.IsSuccessStatusCode.Should().BeTrue();

		var convo = await DB
			.Find<Conversation>()
			.MatchID(Fixture.ConvoID)
			.ExecuteSingleAsync();

		convo!.MemberIDs.Count.Should().Be(1);
		convo!.MemberIDs.First().Should().Be(Fixture.AccountID);
	}

	[Fact, Priority(2)]
	public async Task Already_Member() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.ConvoID
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var convo = await DB
			.Find<Conversation>()
			.MatchID(Fixture.ConvoID)
			.ExecuteSingleAsync();

		var account = await DB
			.Find<User>()
			.MatchID(Fixture.AccountID)
			.ExecuteSingleAsync();

		convo!.MemberIDs.Count.Should().Be(1);
		account!.ConversationIDs.Count.Should().Be(1);
	}

	[Fact, Priority(3)]
	public async Task In_100_Convos() {
		await DB.Update<User>()
			.MatchID(Fixture.AccountID)
			.Modify(u => u.ConversationIDs, Enumerable.Repeat(ObjectId.GenerateNewId().ToString(), 100).ToList())
			.ExecuteAsync();

		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.ConvoID
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}


	[Fact]
	public async Task Nonexistant() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = ObjectId.GenerateNewId().ToString()
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	public async Task Convo_Is_Full() {
		var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
			ConversationID = Fixture.FullConvoID
		});

		rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var convo = await DB
			.Find<Conversation>()
			.MatchID(Fixture.FullConvoID)
			.ExecuteSingleAsync();

		convo!.MemberIDs.Count.Should().Be(100);
		convo!.MemberIDs.Contains(Fixture.FullConvoID).Should().BeFalse();

		var account = await DB.Find<User>().MatchID(Fixture.AccountID).ExecuteFirstAsync();
		account!.ConversationIDs.Contains(convo.ID).Should().BeFalse();
	}
}
