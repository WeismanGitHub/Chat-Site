namespace API.Endpoints.ChatRooms.Get;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

public sealed class ChatRoomDTO {
	public required string ID { get; set; }
	public required string Name { get; set; }
	public required DateTime CreatedAt { get; set; }
}
