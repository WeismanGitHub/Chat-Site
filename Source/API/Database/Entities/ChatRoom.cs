using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;

[Collection("ChatRooms")]
public class ChatRoom : Entity {
	public const int MaxNameLength = 25;
	public const int MinNameLength = 1;
	public const int MaxPasswordLength = 70;
	public const int MinPasswordLength = 10;

	[MaxLength(25, ErrorMessage = "Name cannot be more than 25 characters.")]
	public required string Name { get; set; }
	public required string PasswordHash { get; set; }

	[MaxLength(100, ErrorMessage = "A chat room cannot have more than 100 members")]
	public List<string> MemberIDs { get; set; } = new List<string>();
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
