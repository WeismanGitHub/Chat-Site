using API.Database.Entities;
using MongoDB.Entities;

namespace Tests.API.Endpoints.Account.Signin;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }
}
