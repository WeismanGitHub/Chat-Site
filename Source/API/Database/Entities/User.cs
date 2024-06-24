using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;

[Collection("Users")]
public class User : Entity {
    public const int MaxNameLength = 25;
    public const int MinNameLength = 1;
    public const int MaxPasswordLength = 70;
    public const int MinPasswordLength = 10;

    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
	[MaxLength(100, ErrorMessage = "Cannot join more than 100 chat rooms.")]
	public List<string> ChatRoomIDs { get; set; } = new List<string>();
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
