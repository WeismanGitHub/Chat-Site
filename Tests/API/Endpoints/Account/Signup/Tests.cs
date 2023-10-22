using API.Endpoints.Account.Signup;

namespace Tests.API.Endpoints.Account.Signup;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact]
    public async Task Valid_User_Input() {
        var rsp = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            DisplayName = "Valid Name",
            Email = "valid@email.com",
            Password = "ValidPassword1"
        });
        Console.WriteLine("hello world");

        Fixture.userEmails.Add("valid@email.com");

        rsp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    //[Fact, Priority(1)]
    //public async Task Invalid_User_Input() {
    //    var (rsp, res) = await Fixture.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new() {
    //        DisplayName = "",
    //        LastName = "y"
    //    });

    //    rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    res!.Errors.Count.Should().Be(2);
    //    res.Errors.Keys.Should().Equal("firstName", "lastName");
    //}
}
