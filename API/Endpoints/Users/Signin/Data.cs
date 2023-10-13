namespace API.Endpoints.Users.Signin;

public static class Data {
    internal static Task<User?> GetAccount(string email) {
        return DB
            .Find<User>()
            .Match(u => u.Email == email)
            .Project(u => new() {
                PasswordHash = u.PasswordHash,
                DisplayName = u.DisplayName,
                ID = u.ID,
            })
            .ExecuteSingleAsync();
    }
}
