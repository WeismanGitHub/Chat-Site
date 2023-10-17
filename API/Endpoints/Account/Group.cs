namespace API.Endpoints.Account;
internal class AccountGroup : Group {
    public AccountGroup() {
        Configure("Account", endpoints => {});
    }
}
