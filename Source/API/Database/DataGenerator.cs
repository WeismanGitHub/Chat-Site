//using Bogus;

//using System.Data;

//namespace Database;
//public class DataGenerator {
//    public DataGenerator(IDbConnection db, int seed) {
//        Randomizer.Seed = new Random(seed);
//    }

//    public async void PopulateWithBogus() {
//        if ((await userData.GetAll()).Count > 0) return;

//        var users = await CreateUsers();

//        foreach (var user in users) {
//        }
//    }

//    public async Task<List<UserModel>> CreateUsers() {
//        List<UserModel> users = new();

//        foreach (var fakeUser in userData.faker.GenerateForever().Take(25)) {
//            await userData.CreateUser(fakeUser);
//            users.Add(fakeUser);
//        }

//        return users;
//    }
//}
