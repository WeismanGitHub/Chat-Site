using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;

[Collection("Conversations")]
public class Conversation : Entity {
	public const int MaxNameLength = 25;
	public const int MinNameLength = 1;

	[MaxLength(25, ErrorMessage = "Name cannot be more than 25 characters.")]
	public string Name { get; set; }

	[MaxLength(100, ErrorMessage = "A Conversation cannot have more than 100 members")]
	public List<string> MemberIDs { get; set; } = new List<string>();
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
