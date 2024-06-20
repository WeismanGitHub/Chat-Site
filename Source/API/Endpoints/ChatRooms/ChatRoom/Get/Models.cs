namespace API.Endpoints.ChatRooms.SingleChatRoom.Get;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public required string ChatRoomID { get; set; }

}

public sealed class Member {
	public string ID { get; set; }
	public string Name { get; set; }
}

public sealed class Response {
	public string ID { get; set; }
	public string Name { get; set; }
	public DateTime CreatedAt { get; set; }
	public List<Member> Members { get; set; }
}
