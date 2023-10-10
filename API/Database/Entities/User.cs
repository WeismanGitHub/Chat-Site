using System.ComponentModel.DataAnnotations;

namespace Database.Entities;
public class User : Entity {
    public string Name  = "Users";
    public string CollectionAttribute() {
        return "Users";
    }

    public string DisplayName { get; set; }
    [BsonRequired]
    public string Email { get; set; }
    [BsonRequired]
    public string PasswordHash { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot add more than 100 friends.")]
    public HashSet<string> FriendIds { get; set; } = new();
    [MaxLength(100, ErrorMessage = "Cannot join more than 100 conversations.")]
    public HashSet<string> ConversationIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
