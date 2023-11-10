namespace API.Endpoints.Conversations;
internal class ConversationGroup : Group {
    public ConversationGroup() {
        Configure("Conversations", endpoints => {});
    }
}
