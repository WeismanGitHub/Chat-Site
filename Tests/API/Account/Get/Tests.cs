using GetAPI = API.Endpoints.Account.Get;

namespace Tests.API.Account.Get;

public class Tests : TestClass<Fixture> {
	public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

	[Fact]
	public async Task Default_Request() {
		var (rsp, res) = await Fixture.Client.GETAsync<API.Endpoints.Account.Get, API.Endpoints.Account.Request, API.Endpoints.Account.Response>(new());

		rsp.IsSuccessStatusCode.Should().BeTrue();
		res.ID.Should().Be(Fixture.AccountID);
		res.DisplayName.Should().Be(ValidAccount.DisplayName);
		res.Email.Should().Be(ValidAccount.Email);
		res.TotalConversations.Should().Be(2);
		res.TotalFriends.Should().Be(2);
	}
}
