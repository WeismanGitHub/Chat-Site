using MongoDB.Driver;

namespace Library.DataAccess {
    public interface IDbConnection {
        MongoClient Client { get; }
        IMongoCollection<Conversation> ConversationCollection { get; }
        string ConversationCollectionName { get; }
        string DbName { get; }
        IMongoCollection<FriendRequest> FriendRequestCollection { get; }
        string FriendRequestsCollectionName { get; }
        IMongoCollection<User> UserCollection { get; }
        string UserCollectionName { get; }
    }
}