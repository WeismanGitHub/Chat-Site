import ky from 'ky';

class Account {
    public static get(): Promise<AccountData> {
        return ky.get('/API/Account/v1').json();
    }

    public static signin(data: { email: string; password: string }) {
        return ky.post('/API/Account/Signin/v1', { json: data }).json();
    }

    public static signup(data: { displayName: string; email: string; password: string }) {
        return ky.post('/API/Account/Signup/v1', { json: data }).json();
    }

    public static update(data: {
        currentPassword: string;
        newData: {
            displayName: string | null;
            email: string | null;
            password: string | null;
        };
    }) {
        return ky.patch('API/Account/v1', { json: data }).json();
    }

    public static delete() {
        return ky.delete('/API/Account/v1').json();
    }

    public static signout() {
        return ky.post('/API/Account/Signout/v1').json();
    }
}

class Requests {
    public static get({
        type = 'Incoming',
        page = 0,
    }: {
        type?: 'Incoming' | 'Outgoing';
        page?: number;
    }): Promise<{ friendRequests: FriendRequest[], totalCount: number }> {
        return ky // implement FriendReqDTO
            .get('/API/Friends/Requests/v1', {
                searchParams: {
                    type,
                    page,
                },
            })
            .json();
    }
}

class Friends {
    public static Requests = Requests;

    public static get(): Promise<Friend[]> {
        return ky.get('/API/Friends/v1').json();
    }

    public static remove(friendID: string) {
        return ky.post(`/API/Friends/${friendID}/remove/v1`);
    }
}

class Conversations {
    public static get(): Promise<ConversationsData> {
        return ky.get('/API/Conversations/v1').json();
    }

    public static leave(conversationID: string) {
        return ky.post(`/API/Conversations/${conversationID}/leave/v1`);
    }

    public static getOne(conversationID: string): Promise<SingleConvoData> {
        return ky.get(`/API/Conversations/${conversationID}/v1`).json();
    }
}

export default class Endpoints {
    public static readonly Account = Account;
    public static readonly Friends = Friends;
    public static readonly Conversations = Conversations;
}
