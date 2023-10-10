namespace Database.Entities;

public class Conversation : Entity {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Name { get; set; }
    public HashSet<string> MemberIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
