namespace API.Endpoints.Friends;
internal class FriendGroup : Group {
    public FriendGroup() {
        Configure("Conversations", endpoints => {});
    }
}
