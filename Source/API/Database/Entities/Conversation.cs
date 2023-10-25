namespace API.Database.Entities;

[Collection("Conversations")]
public class Conversation : Entity {
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Name { get; set; }
    public Many<User> Members { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CollectionAttribute() {
        return "Conversations";
    }

    public Conversation() {
        this.InitManyToMany(() => Members, member => member.ID);
    }
}
