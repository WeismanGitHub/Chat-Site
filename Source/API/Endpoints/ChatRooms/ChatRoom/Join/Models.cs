namespace API.Endpoints.ChatRooms.SingleChatRoom.Join;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public required string ChatRoomID { get; set; }

}
