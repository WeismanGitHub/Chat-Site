using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;

[Collection("Users")]
public class User : Entity {
    public const int MaxNameLength = 50;
    public const int MinNameLength = 1;
    public const int MaxPasswordLength = 70;
    public const int MinPasswordLength = 10;

    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot add more than 100 friends.")]
    public Many<User> Friends { get; set; }
    [MaxLength(100, ErrorMessage = "Cannot join more than 100 conversations.")]
    public Many<Conversation> Conversations { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User() {
        this.InitOneToMany(() => Friends);
        //this.InitManyToMany(() => Conversations, convo => convo.Members);
    }
}
