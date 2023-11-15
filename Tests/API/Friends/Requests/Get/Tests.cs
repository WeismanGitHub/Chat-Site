using API.Endpoints.Friends.Requests.Get;
using API.Database.Entities;

namespace Tests.API.Friends.Requests.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Default_Request() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new());

		//rsp.IsSuccessStatusCode.Should().BeTrue();
		rsp.StatusCode.Should().Be(HttpStatusCode.OK);
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RecipientID.Should().Be(Fixture.AccountID);
		}
	}

	[Fact]
	public async Task First_Page_Specified() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			Page = 0
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public async Task Second_Page() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			Page = 1
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}
	
	[Fact]
	public async Task Third_Page() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			Page = 2
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public async Task Empty_Page() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			Page = 50
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public async Task Outgoing_Requests() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			FriendReqType = FriendRequestType.Outgoing
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public async Task Incoming_Requests() {
		var res = await Fixture.Client.GETAsync<Endpoint, Request>(new() {
			AccountID = Fixture.AccountID,
			FriendReqType = FriendRequestType.Incoming
		});

		res.IsSuccessStatusCode.Should().BeTrue();
	}
}
