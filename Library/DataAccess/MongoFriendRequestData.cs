namespace Library.DataAccess;

public interface IFriendRequestData {
    void AcceptFriendRequest(FriendRequest friendRequest);
    void DeclineFriendRequest(FriendRequest friendRequest);
    void DeleteFriendRequest(FriendRequest friendRequest);
}

public class MongoFriendRequestData : IFriendRequestData {
    private readonly IMongoCollection<FriendRequest> _friendRequests;
    public MongoFriendRequestData(DbConnection db) {
        _friendRequests = db.FriendRequestCollection;
    }

    public void AcceptFriendRequest(FriendRequest friendRequest) {

    }
    public void DeclineFriendRequest(FriendRequest friendRequest) {

    }
    public void DeleteFriendRequest(FriendRequest friendRequest) {

    }
}
