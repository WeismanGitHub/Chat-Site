namespace API.Database;

public class _001_nothing: IMigration {
    public Task UpgradeAsync() {
        return Task.CompletedTask;
    }
}
