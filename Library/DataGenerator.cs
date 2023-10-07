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

        foreach (var fakeUser in userData.faker.GenerateForever().Take(10)) {
            await userData.CreateUser(fakeUser);
        }
    }
}
