namespace Library.DataAccess;

public interface IFriendRequestData {
    void AcceptFriendRequest(FriendRequestModel friendRequest);
    void DeclineFriendRequest(FriendRequestModel friendRequest);
    void DeleteFriendRequest(FriendRequestModel friendRequest);
}

public class MongoFriendRequestData : IFriendRequestData {
    private readonly IMongoCollection<FriendRequestModel> _friendRequests;
    public MongoFriendRequestData(IDbConnection db) {
        _friendRequests = db.FriendRequestCollection;
    }

    public void AcceptFriendRequest(FriendRequestModel friendRequest) {

    }
    public void DeclineFriendRequest(FriendRequestModel friendRequest) {

    }
    public void DeleteFriendRequest(FriendRequestModel friendRequest) {

    }
}
