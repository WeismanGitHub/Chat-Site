using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;
public enum Status {
    Accepted,
    Declined,
    Pending
}

[Collection("FriendRequests")]
public class FriendRequest: Entity {
    [BsonRepresentation(BsonType.ObjectId)]
    public string RequesterID { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecipientID { get; set; }
    [MaxLength(250)]
    public string? Message { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
