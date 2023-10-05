namespace Library.Models;
public class UserModel {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; }
    [BsonRequired]
    public string DisplayName { get; set; }
    [BsonRequired]
    public string Email { get; set; }
    public HashSet<string> FriendIds { get; set; } = new();
    public HashSet<string> ConversationIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
