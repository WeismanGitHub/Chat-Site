﻿using Microsoft.Extensions.Configuration;

namespace Library.DataAccess;

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

public class DbConnection : IDbConnection {
    private string _connectionId = "MongoDB";
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;

    public string DbName { get; private set; }
    public string FriendRequestsCollectionName { get; private set; } = "FriendRequests";
    public string ConversationCollectionName { get; private set; } = "Conversations";
    public string UserCollectionName { get; private set; } = "Users";

    public MongoClient Client { get; private set; }
    public IMongoCollection<FriendRequest> FriendRequestCollection { get; private set; }
    public IMongoCollection<Conversation> ConversationCollection { get; private set; }
    public IMongoCollection<User> UserCollection { get; private set; }
    public DbConnection(IConfiguration config) {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"]!;
        _db = Client.GetDatabase(DbName);

        CreateCollections();
        CreateIndexes();
    }

    private void CreateCollections() {
        FriendRequestCollection = _db.GetCollection<FriendRequest>(FriendRequestsCollectionName);
        ConversationCollection = _db.GetCollection<Conversation>(ConversationCollectionName);
        UserCollection = _db.GetCollection<User>(UserCollectionName);
    }

    private void CreateIndexes() {
        FriendRequestCollection.Indexes.CreateOne(
            new CreateIndexModel<FriendRequest>(
                Builders<FriendRequest>.IndexKeys.Ascending(req => req.RecipientId)
            )
        );

        UserCollection.Indexes.CreateOne(
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.Email),
                new CreateIndexOptions() { Unique = true }
            )
        );
    }
}
