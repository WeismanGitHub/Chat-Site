import ky from 'ky';

class Account {
    public static get(): Promise<accountData> {
        return ky.get('/API/Account/v1').json();
    }

    public static signin() {
        return ky.post('/API/Account/Signin/v1').json();
    }

    public static signup() {
        return ky.post('/API/Account/Signup/v1').json();
    }

    public static signout() {
        return ky.post('/API/Account/Signout/v1').json();
    }
}

class Friends {
    public static get(): Promise<friend[]> {
        return ky.get('/API/Friends/v1').json();
    }

    public static remove(friendID: string) {
        return ky.post(`/API/Friends/${friendID}/remove/v1`);
    }
}

class Conversations {
    public static get(): Promise<conversation[]> {
        return ky.get('/API/Conversations/v1').json();
    }

    public static leave(conversationID: string) {
        return ky.post(`/API/Conversations/${conversationID}/leave/v1`);
    }

    public static getOne(conversationID: string) {
        return ky.get(`/API/Conversations/${conversationID}/v1`).json();
    }
}

export default class Endpoints {
    public static readonly Account = Account;
    public static readonly Friends = Friends;
    public static readonly Conversations = Conversations;
}
