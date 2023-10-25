namespace API.Endpoints.Friends.Requests;
internal class RequestGroup : Group {
    public RequestGroup() {
        Configure("Friends/Requests", endpoints => { });
    }
}
