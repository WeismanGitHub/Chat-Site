namespace Library.DataAccess; 
public class MongoFriendRequestData {
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
