namespace API.Endpoints.Friends.Get;

public static class Data {
    internal static async Task<List<User>> GetFriends(User user) {
        foreach (var friend in user.Friends) {
            Console.WriteLine(friend.ID);
        }

        return await DB
            .Find<User>()
            .Match(u => user.Friends.Any(f => f.ID == u.ID))
            .Project(u => new() {
                DisplayName = u.DisplayName,
                ID = u.ID,
                CreatedAt = u.CreatedAt
            })
            .ExecuteAsync();
    }
}
