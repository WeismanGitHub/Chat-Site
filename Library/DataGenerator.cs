using Bogus;

namespace Library;
public class DataGenerator {
    private MongoFriendRequestData friendRequestData { get; }
    private MongoConversationData conversationData { get; }
    private MongoUserData userData { get; set; }
    public DataGenerator(IDbConnection db, int seed) {
        Randomizer.Seed = new Random(seed);

        friendRequestData = new MongoFriendRequestData(db);
        conversationData = new MongoConversationData(db);
        userData = new MongoUserData(db);
    }

    public async void PopulateWithBogus() {
        if ((await userData.GetAll()).Count > 0) return;
        
        var users = await CreateUsers();

        foreach (var user in users) {
            Console.WriteLine(user.Id);
        }
    }

    public async Task<List<UserModel>> CreateUsers() {
        List<UserModel> users = new();

        foreach (var fakeUser in userData.faker.GenerateForever().Take(25)) {
            await userData.CreateUser(fakeUser);
            users.Add(fakeUser);
        }

        return users;
    }
}
