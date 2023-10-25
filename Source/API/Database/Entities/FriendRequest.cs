using System.ComponentModel.DataAnnotations;

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
    public string RequesterID { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecipientID { get; set; }
    [MaxLength(250)]
    public string? Message { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public FriendRequest() {
        //this.InitOneToMany(() => RecipientID, member => member.ID);
    }

    public async void AcceptFriendRequest(User? user) {
        if (user == null) {
            user = await DB.Find<User>().OneAsync(RecipientID);
        }
        
        if (user?.ID != RecipientID) throw new UnauthorizedAccessException();

        Status = Status.Accepted;
        await user.Friends.AddAsync(user);
    }
    public void DeclineFriendRequest(User user) {
        if (user.ID != RecipientID) throw new UnauthorizedAccessException();
    }
    public void DeleteFriendRequest(FriendRequest friendRequest) {

    }
}
