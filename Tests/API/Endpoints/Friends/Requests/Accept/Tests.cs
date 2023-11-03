using API.Endpoints.Friends.Requests.Accept;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.Friends.Requests.Accept;

public class Tests : TestClass<Fixture> {
    public Tests(Fixture fixture, ITestOutputHelper output) : base(fixture, output) { }

    [Fact, Priority(1)]
    public async Task Valid_Friend_Request() {
        var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
            AccountID = Fixture.UserID1,
			RequestID = Fixture.RequestID1,
        });

		var user1 = await DB.Find<User>().MatchID(Fixture.UserID1).ExecuteSingleAsync();
		var user2 = await DB.Find<User>().MatchID(Fixture.UserID2).ExecuteSingleAsync();
		var friendRequest = await DB.Find<FriendRequest>().MatchID(Fixture.RequestID1).ExecuteSingleAsync();

        res.IsSuccessStatusCode.Should().BeTrue();

		user1.Should().NotBeNull();
		user2.Should().NotBeNull();
		friendRequest.Should().NotBeNull();

		user1!.FriendIDs.Contains(Fixture.UserID2).Should().BeTrue();
		user2!.FriendIDs.Contains(Fixture.UserID1).Should().BeTrue();
		friendRequest!.Status.Should().Be(Status.Accepted);
    }

	//[Fact]
	//public async Task Valid_No_Message() {
	//	var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//		AccountID = Fixture.UserID1,
	//		RecipientID = Fixture.UserID2
	//	});

	//	res.IsSuccessStatusCode.Should().BeTrue();
	//}

	//[Fact]
 //   public async Task Duplicated_Friend_Request() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = "Let's be friends.",
 //           RecipientID = Fixture.UserID2
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }

	//[Fact]
	//public async Task Request_To_Self() {
	//	var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
	//		AccountID = Fixture.UserID1,
	//		Message = "Let's be friends.",
	//		RecipientID = Fixture.UserID1
	//	});

	//	res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}

	//[Fact]
	//public async Task Already_Friends() {
	//	await DB.Update<User>()
	//		.Modify(u => u.FriendIDs, new List<string> { Fixture.UserID1 })
	//		.MatchID(Fixture.UserID2).ExecuteAsync();

 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = "Let's be friends.",
 //           RecipientID = Fixture.UserID2
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }

 //   [Fact]
 //   public async Task Invalid_RecipientID() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = "Let's be friends.",
 //           RecipientID = "invalid id"
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }
    
 //   [Fact]
 //   public async Task Invalid_Account() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = ObjectId.GenerateNewId().ToString(),
 //           Message = "Let's be friends.",
 //           RecipientID = Fixture.UserID2
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }

 //   [Fact]
 //   public async Task Invalid_Recipient() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = "Let's be friends.",
 //           RecipientID = ObjectId.GenerateNewId().ToString()
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }

 //   [Fact]
 //   public async Task Message_Too_Long() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = new string('*', 251),
 //           RecipientID = "invalid id"
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }

 //   [Fact]
 //   public async Task Empty_Message() {
 //       var res = await Fixture.Client.POSTAsync<Endpoint, Request>(new() {
 //           AccountID = Fixture.UserID1,
 //           Message = "",
 //           RecipientID = "invalid id"
 //       });

 //       res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 //   }/
}
