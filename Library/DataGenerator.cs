using Bogus;

namespace Library;
public class DataGenerator {
    public DataGenerator(int seed) {
        Randomizer.Seed = new Random(seed);
    }

    public async Task<bool> CollectionsAreEmpty() {
        return true;
    }

    public async Task PopulateDatabaseWithBogus() {

    }
}
