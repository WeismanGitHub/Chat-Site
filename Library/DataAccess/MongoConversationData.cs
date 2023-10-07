namespace Library.DataAccess;

public interface IConversationData : ICollectionData<ConversationModel> {
    ConversationModel CreateConversation(HashSet<string> memberIds, string Name);
}

public class MongoConversationData : IConversationData {
    private readonly IMongoCollection<ConversationModel> _conversations;
    public MongoConversationData(IDbConnection db) {
        _conversations = db.ConversationCollection;
    }

    public async Task<List<ConversationModel>> GetAll() {
        var results = await _conversations.FindAsync(_ => true);

        return results.ToList();
    }

    public ConversationModel CreateConversation(HashSet<string> memberIds, string Name) {
        var convo = new ConversationModel {
            MemberIds = memberIds,
            Name = Name
        };

        return convo;
    }
}
