namespace Library.DataAccess;

public interface IConversationData {
    ConversationModel CreateConversation(HashSet<string> memberIds, string Name);
}

public class MongoConversationData : IConversationData {
    private readonly IMongoCollection<ConversationModel> _conversations;
    public MongoConversationData(IDbConnection db) {
        _conversations = db.ConversationCollection;
    }
    public ConversationModel CreateConversation(HashSet<string> memberIds, string Name) {
        var convo = new ConversationModel {
            MemberIds = memberIds,
            Name = Name
        };

        return convo;
    }
}