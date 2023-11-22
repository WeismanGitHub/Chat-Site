class Account {
    public static Route() {
        return '/API/Account/v1';
    }

    public static Signin() {
        return '/API/Account/Signin/v1';
    }

    public static Signup() {
        return '/API/Account/Signup/v1';
    }

    public static Signout() {
        return '/API/Account/Signout/v1';
    }
}

class Friends {
    public static Route() {
        return '/API/Friends/v1';
    }

    public static Remove(friendID: string) {
        return `/API/Friends/${friendID}/remove/v1`;
    }
}

class Conversations {}

export default class Endpoints {
    public static readonly Account = Account;
    public static readonly Friends = Friends;
    public static readonly Conversations = Conversations;
}
