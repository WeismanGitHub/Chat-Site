namespace API.Endpoints.Account.Update;

public static class Data {
    internal static Task<bool> Update(AccountData account) {
        return DB
            .Find<User>()
            .Match(u => u.ID == account.ID)
            .ExecuteAnyAsync();
    }
}
