namespace API.Endpoints.ChatRooms;
internal class ChatRoomGroup : Group {
    public ChatRoomGroup() {
        Configure("ChatRooms", endpoints => {});
    }
}
