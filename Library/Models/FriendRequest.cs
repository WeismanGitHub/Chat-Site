namespace Library.Models;
public class FriendRequest {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string SenderId { get; set; }
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
