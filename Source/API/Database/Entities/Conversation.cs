using System.ComponentModel.DataAnnotations;

namespace API.Database.Entities;

[Collection("Conversations")]
public class Conversation : Entity {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Name { get; set; }

	[MaxLength(100, ErrorMessage = "Cannot have more than 100 members.")]
	public List<string> MemberIDs { get; set; } = new List<string>();
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CollectionAttribute() {
        return "Conversations";
    }
}
