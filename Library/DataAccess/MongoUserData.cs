using Bogus;

namespace Library.DataAccess;

public interface IUserData : ICollectionData<UserModel> {
    Task CreateUser(UserModel user);
    Task<UserModel> GetUser(string id);
    Task<UserModel> GetUserFromAuthentication(string objectId);
    Task UpdateUser(UserModel user);
}

public class MongoUserData : IUserData {
    private readonly IMongoCollection<UserModel> _users;
    public readonly Faker<UserModel> faker;

    public MongoUserData(IDbConnection db) {
        _users = db.UserCollection;
        var x = _users.Find(x => true);

        faker = new Faker<UserModel>()
            .RuleFor(u => u.ObjectIdentifier, _ => Guid.NewGuid().ToString())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.DisplayName, f => f.Internet.UserName());
    }

    public async Task<List<UserModel>> GetAll() {
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
