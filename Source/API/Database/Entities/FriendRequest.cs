using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace API.Database.Entities;
public enum Status {
    Accepted,
    Declined,
    Pending
}

[Collection("FriendRequests")]
public class FriendRequest: Entity {
    [BsonRepresentation(BsonType.ObjectId)]
    public string RequesterId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecipientId { get; set; }
    public string Message { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public FriendRequest() {
        //this.InitOneToMany(() => RecipientId, member => member.ID);
    }

    public async void AcceptFriendRequest(User? user) {
        if (user == null) {
            user = await DB.Find<User>().OneAsync(RecipientId);
        }
        
        if (user?.ID != RecipientId) throw new UnauthorizedAccessException();

        Status = Status.Accepted;
        await user.Friends.AddAsync(user);
    }
    public void DeclineFriendRequest(User user) {
        if (user.ID != RecipientId) throw new UnauthorizedAccessException();
    }
    public void DeleteFriendRequest(FriendRequest friendRequest) {

    }
}
