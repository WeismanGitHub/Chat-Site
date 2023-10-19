namespace API.Endpoints.Account.Update;

public static class Data {
    internal static Task Update(Request newData) {
        var update = DB.Update<User>();

        if (newData.DisplayName != null) {
            update.Modify(u => u.DisplayName, newData.DisplayName);
        }

        if (newData.Email != null) {
            update.Modify(u => u.Email, newData.Email);
        }

        if (newData.DisplayName != null) {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newData.Password);
            update.Modify(u => u.PasswordHash, passwordHash);
        }
        
        return update.ExecuteAsync();
    }
}
