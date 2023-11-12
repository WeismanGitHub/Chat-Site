namespace API.Endpoints.Account.Delete;

public static class Data {
    internal static async Task DeleteUser(User user) {
        var conversations = await DB.Find<Conversation>().ManyAsync(convo => 
            user.ConversationIDs.Any(id => id == convo.ID)
        );

        var transaction = new Transaction();
        
        await transaction.DeleteAsync<User>(user.ID);
        //await transaction.Update<Conversation>().Match(convo => user.Conversations.)

        await transaction.CommitAsync();
    }
}
