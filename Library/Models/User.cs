namespace Library.Models;
public class User {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public HashSet<string> FriendIds { get; set; } = new();
    public HashSet<string> ConversationIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //public List<User> GetFriends() {
    //}
}