namespace Library.Models;

public enum Status {
    Accepted,
    Declined,
    Pending
}
public class FriendRequestModel {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RequesterId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecipientId { get; set; }
    public string Message { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
