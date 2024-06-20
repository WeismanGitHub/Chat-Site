using Signin = API.Endpoints.Account.Signin;
using API.Database.Entities;
using MongoDB.Entities;
using MongoDB.Bson;

namespace Tests.API.Friends.Requests.Accept;

public class Fixture : TestFixture<Program> {
	public Fixture(IMessageSink sink) : base(sink) { }
	public readonly string UserID1 = ObjectId.GenerateNewId().ToString();
	public readonly string UserID2 = ObjectId.GenerateNewId().ToString();
	public readonly string UserID3 = ObjectId.GenerateNewId().ToString();

	public readonly string RequestID1 = ObjectId.GenerateNewId().ToString();
	public readonly string RequestID2 = ObjectId.GenerateNewId().ToString();
	public readonly string RequestID3 = ObjectId.GenerateNewId().ToString();
	public readonly string RequestID4 = ObjectId.GenerateNewId().ToString();

	protected override async Task SetupAsync() {
		await DB.InsertAsync(new List<User> {
			new () {
				ID = UserID1,
				DisplayName = ValidAccount.DisplayName,
				Email = ValidAccount.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
			},
			new () {
				ID = UserID2,
				DisplayName = ValidAccount.DisplayName,
				Email = "2@email.com",
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
			},
			new () {
				ID = UserID3,
				DisplayName = ValidAccount.DisplayName,
				Email = "3@email.com",
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(ValidAccount.Password)
			}
		});

		await DB.InsertAsync(new List<FriendRequest>() {
			new () {
				ID = RequestID1,
				RecipientID = UserID1,
				RequesterID = UserID2,
			},
			new () {
				ID = RequestID2,
				RecipientID = UserID2,
				RequesterID = UserID3
			},
			new () {
				ID = RequestID3,
				RecipientID = UserID1,
				RequesterID = ObjectId.GenerateNewId().ToString(),
			},
			new () {
				ID = RequestID4,
				RecipientID = UserID1,
				RequesterID = UserID3,
			}
		});

		await Client.POSTAsync<API.Endpoints.Account.Signin, API.Endpoints.Account.Request>(new Signin.Request() {
			Email = ValidAccount.Email,
			Password = ValidAccount.Password,
		});
	}

	protected override async Task TearDownAsync() {
		await DB.DeleteAsync<FriendRequest>(new List<string>() { RequestID1, RequestID2, RequestID3, RequestID4 });
		await DB.DeleteAsync<User>(new List<string>() { UserID1, UserID2, UserID3 });
	}
}
