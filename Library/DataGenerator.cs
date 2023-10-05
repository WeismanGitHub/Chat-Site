using Bogus;

namespace Library;
internal class DataGenerator {
    public DataGenerator() {
        Randomizer.Seed = new Random(1234);
    }
}
