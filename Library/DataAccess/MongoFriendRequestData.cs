using Bogus;

namespace Library.DataAccess;

public interface IFriendRequestData : ICollectionData<FriendRequestModel> {
    void AcceptFriendRequest(FriendRequestModel friendRequest);
    void DeclineFriendRequest(FriendRequestModel friendRequest);
    void DeleteFriendRequest(FriendRequestModel friendRequest);
}

public class MongoFriendRequestData : IFriendRequestData {
    private readonly IMongoCollection<FriendRequestModel> _friendRequests;
    public readonly Faker<FriendRequestModel> faker;

    public MongoFriendRequestData(IDbConnection db) {
        _friendRequests = db.FriendRequestCollection;

        faker = new Faker<FriendRequestModel>()
            .RuleFor(f => f.Message, f => f.Lorem.Sentence())
            .RuleFor(u => u.Status, f => f.PickRandom<Status>());
    }
    public async Task<List<FriendRequestModel>> GetAll() {
        var results = await _friendRequests.FindAsync(_ => true);

        return results.ToList();
    }

    public void AcceptFriendRequest(FriendRequestModel friendRequest) {

    }
    public void DeclineFriendRequest(FriendRequestModel friendRequest) {

    }
    public void DeleteFriendRequest(FriendRequestModel friendRequest) {

    }
}
