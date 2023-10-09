namespace Database.Entities;

public enum Status {
    Accepted,
    Declined,
    Pending
}
public class FriendRequest: Entity {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RequesterId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecipientId { get; set; }
    public string Message { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public async void AcceptFriendRequest(User user) {
        if (user.Id != RecipientId) throw new UnauthorizedAccessException();

        Status = Status.Accepted;
        user.FriendIds.Add(RequesterId);
    }
    public void DeclineFriendRequest(User user) {
        if (user.Id != RecipientId) throw new UnauthorizedAccessException();
    }
    public void DeleteFriendRequest(FriendRequest friendRequest) {

    }
}
