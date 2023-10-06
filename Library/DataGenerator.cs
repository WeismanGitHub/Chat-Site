using Bogus;

namespace Library;
public class DataGenerator {
    private Faker<UserModel> fakeUserModel;
    public DataGenerator(int seed) {
        Randomizer.Seed = new Random(seed);

        fakeUserModel = new Faker<UserModel>()
            .RuleFor(u => u.ObjectIdentifier, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.DisplayName, f => f.Internet.UserName());
    }

    public async Task<bool> CollectionsAreEmpty() {
        return true;
    }

    public async Task PopulateDatabaseWithBogus() {

    }
}
