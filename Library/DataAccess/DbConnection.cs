using Microsoft.Extensions.Configuration;

namespace Library.DataAccess;

public interface IDbConnection {
    MongoClient Client { get; }
    IMongoCollection<ConversationModel> ConversationCollection { get; }
    string ConversationCollectionName { get; }
    string DbName { get; }
    IMongoCollection<FriendRequestModel> FriendRequestCollection { get; }
    string FriendRequestsCollectionName { get; }
    IMongoCollection<UserModel> UserCollection { get; }
    string UserCollectionName { get; }
}

public class DbConnection : IDbConnection {
    private readonly IMongoDatabase _db;

    public string DbName { get; private set; }
    public string FriendRequestsCollectionName { get; private set; } = "FriendRequests";
    public string ConversationCollectionName { get; private set; } = "Conversations";
    public string UserCollectionName { get; private set; } = "Users";

    public MongoClient Client { get; private set; }
    public IMongoCollection<FriendRequestModel> FriendRequestCollection { get; private set; }
    public IMongoCollection<ConversationModel> ConversationCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public DbConnection(IConfiguration config) {
        Client = new MongoClient(config["MongoURI"]);
        DbName = config["DatabaseName"]!;
        _db = Client.GetDatabase(DbName);

        CreateCollections();
        CreateIndexes();
    }

    private void CreateCollections() {
        FriendRequestCollection = _db.GetCollection<FriendRequestModel>(FriendRequestsCollectionName);
        ConversationCollection = _db.GetCollection<ConversationModel>(ConversationCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
    }

    private void CreateIndexes() {
        FriendRequestCollection.Indexes.CreateOne(
            new CreateIndexModel<FriendRequestModel>(
                Builders<FriendRequestModel>.IndexKeys.Ascending(req => req.RecipientId)
            )
        );

        UserCollection.Indexes.CreateOne(
            new CreateIndexModel<UserModel>(
                Builders<UserModel>.IndexKeys.Ascending(user => user.Email),
                new CreateIndexOptions() { Unique = true }
            )
        );
    }
}
