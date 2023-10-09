using System.ComponentModel.DataAnnotations;

namespace Database.Entities;
public class User : Entity {
    public string Name  = "Users";
    public string CollectionAttribute(string name) {
        return "Users";
    }

    [BsonId, BsonRepresentation(BsonType.ObjectId), Required]
    public string Id { get; set; }
    [Required]
    public string DisplayName { get; set; }
    [BsonRequired, Required]
    public string Email { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot add more than 100 friends."), Required]
    public HashSet<string> FriendIds { get; set; } = new();
    [MaxLength(100, ErrorMessage = "Cannot join more than 100 conversations."), Required]
    public HashSet<string> ConversationIds { get; set; } = new();
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
