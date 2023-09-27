namespace Library.DataAccess;

public interface IConversationData {
    Conversation CreateConversation(HashSet<string> memberIds, string Name);
}

public class MongoConversationData : IConversationData {
    private readonly IMongoCollection<Conversation> _conversations;
    public MongoConversationData(IDbConnection db) {
        _conversations = db.ConversationCollection;
    }
    public Conversation CreateConversation(HashSet<string> memberIds, string Name) {
        var convo = new Conversation {
            MemberIds = memberIds,
            Name = Name
        };

        return convo;
    }
}