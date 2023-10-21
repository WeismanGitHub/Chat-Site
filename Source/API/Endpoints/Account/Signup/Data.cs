namespace API.Endpoints.Account.Signup;

public static class Data {
    internal static Task<bool> EmailAddressIsTaken(string email) {
        return DB
            .Find<User>()
            .Match(u => u.Email == email)
            .ExecuteAnyAsync();
    }
}
