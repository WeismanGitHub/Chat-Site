using System.ComponentModel.DataAnnotations;

namespace Database.Models;
public class UserModel {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; }
    [BsonRequired]
    public string DisplayName { get; set; }
    [BsonRequired]
    public string Email { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot add more than 100 friends.")]
    public HashSet<string> FriendIds { get; set; } = new();
    [MaxLength(100, ErrorMessage = "Cannot join more than 100 conversations.")]
    public HashSet<string> ConversationIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
