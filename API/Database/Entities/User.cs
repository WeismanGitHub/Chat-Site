using System.ComponentModel.DataAnnotations;

namespace Database.Entities;
public class User : Entity {
    public static readonly int MaxNameLength = 50;
    public static readonly int MinNameLength = 1;
    public static readonly int MaxPasswordLength = 70;
    public static readonly int MinPasswordLength = 10;

    public string CollectionAttribute() {
        return "Users";
    }

    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot add more than 100 friends.")]
    public HashSet<string> FriendIds { get; set; } = new();
    [MaxLength(100, ErrorMessage = "Cannot join more than 100 conversations.")]
    public HashSet<string> ConversationIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
