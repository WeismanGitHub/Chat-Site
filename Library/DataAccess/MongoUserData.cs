using ZstdSharp.Unsafe;

namespace Library.DataAccess;
public class MongoUserData {
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
    public async Task<User> GetUserFromAuthentication(string id) {
        var user = await _users.FindAsync(user => user.Id == id);

        return user.FirstOrDefault();
    }
}
