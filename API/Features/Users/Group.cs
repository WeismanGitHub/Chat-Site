namespace API.Features.Users;
internal class UsersGroup : Group {
    public UsersGroup() {
        Configure("Users", endpoints => {});
    }
}
