namespace API.Features.Users.Signup;

public static class Data {
    internal static Task<bool> EmailAddressIsTaken(string email) {
        return DB
            .Find<User>()
            .Match(a => a.Email == email)
            .ExecuteAnyAsync();
    }

    internal static Task CreateNewUser(User user) {
        return user.SaveAsync();
    }
}
