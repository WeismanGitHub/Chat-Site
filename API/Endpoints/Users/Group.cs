namespace API.Endpoints.Users;
internal class UsersGroup : Group {
    public UsersGroup() {
        Configure("Users", endpoints => {});
    }
}
