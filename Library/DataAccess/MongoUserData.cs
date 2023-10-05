namespace Library.DataAccess;

public interface IUserData {
    Task CreateUser(UserModel user);
    Task<UserModel> GetUser(string id);
    Task<UserModel> GetUserFromAuthentication(string objectId);
    Task<List<UserModel>> GetUsers();
    Task UpdateUser(UserModel user);
}

public class MongoUserData : IUserData {
    private readonly IMongoCollection<UserModel> _users;

    public MongoUserData(IDbConnection db) {
        _users = db.UserCollection;
    }

    public async Task<List<UserModel>> GetUsers() {
        var results = await _users.FindAsync(_ => true);

        return results.ToList();
    }

    public async Task<UserModel> GetUser(string id) {
        var user = await _users.FindAsync(user => user.Id == id);

        return user.FirstOrDefault();
    }
    public async Task<UserModel> GetUserFromAuthentication(string objectId) {
        var user = await _users.FindAsync(user => user.ObjectIdentifier == objectId);

        return user.FirstOrDefault();
    }

    public Task CreateUser(UserModel user) {
        return _users.InsertOneAsync(user);
    }

    public Task UpdateUser(UserModel user) {
        var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
        var options = new ReplaceOptions { IsUpsert = true };

        return _users.ReplaceOneAsync(filter, user, options);
    }
}
