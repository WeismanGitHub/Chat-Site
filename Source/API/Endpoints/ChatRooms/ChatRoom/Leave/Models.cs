namespace API.Endpoints.ChatRooms.SingleChatRoom.Leave;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public required string AccountID { get; set; }
	public required string ChatRoomID { get; set; }

}
