using ZstdSharp.Unsafe;

namespace Library.DataAccess;
public class MongoUserData : IUserData {
    private readonly IMongoCollection<User> _users;

    public MongoUserData(IDbConnection db) {
        _users = db.UserCollection;
    }

    public async Task<List<User>> GetUsers() {
        var results = await _users.FindAsync(_ => true);

        return results.ToList();
    }

    public async Task<User> GetUser(string id) {
        var user = await _users.FindAsync(user => user.Id == id);

        return user.FirstOrDefault();
    }
    public async Task<User> GetUserFromAuthentication(string objectId) {
        var user = await _users.FindAsync(user => user.ObjectIdentifier == objectId);

        return user.FirstOrDefault();
    }

    public Task CreateUser(User user) {
        return _users.InsertOneAsync(user);
    }

    public Task UpdateUser(User user) {
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        var options = new ReplaceOptions { IsUpsert = true };

        return _users.ReplaceOneAsync(filter, user, options);
    }
}
