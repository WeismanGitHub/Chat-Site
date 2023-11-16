using API.Endpoints.Friends.Requests.Get;

namespace Tests.API.Friends.Requests.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Default_Request() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new());

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RecipientID.Should().Be(Fixture.AccountID);
		}
	}

	[Fact]
	public async Task First_Page() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new() {  Page = 1 });

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RecipientID.Should().Be(Fixture.AccountID);
		}
	}

	[Fact]
	public async Task Second_Page() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new() { Page = 2 });

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RecipientID.Should().Be(Fixture.AccountID);
		}
	}

	[Fact]
	public async Task Third_Page() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new() { Page = 3 });

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(1);

		res.FriendRequests.First().RecipientID.Should().Be(Fixture.AccountID);
	}

	[Fact]
	public async Task Empty_Page() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(new() { Page = 50 });

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(0);
	}

	[Fact]
	public async Task Outgoing_Requests() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(
			new() { Type = FriendRequestType.Outgoing }
		);

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RequesterID.Should().Be(Fixture.AccountID);
		}
	}

	[Fact]
	public async Task Incoming_Requests() {
		var (rsp, res) = await Fixture.Client.GETAsync<Endpoint, Request, Response>(
			new() { Type = FriendRequestType.Incoming }
		);

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.FriendRequests.Count.Should().Be(10);

		foreach (var friendReq in res.FriendRequests) {
			friendReq.RecipientID.Should().Be(Fixture.AccountID);
		}
	}
}
