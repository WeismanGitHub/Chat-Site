import axios from 'axios';

class Account {
    public static get() {
        return axios.get<AccountData>('/API/Account/v1');
    }

    public static signin(data: { email: string; password: string }) {
        return axios.post('/API/Account/Signin/v1', data);
    }

    public static signup(data: { displayName: string; email: string; password: string }) {
        return axios.post('/API/Account/Signup/v1', data);
    }

    public static update(data: {
        currentPassword: string;
        newData: {
            displayName: string | null;
            email: string | null;
            password: string | null;
        };
    }) {
        return axios.patch('API/Account/v1', data);
    }

    public static delete() {
        return axios.delete('/API/Account/v1');
    }

    public static signout() {
        return axios.post('/API/Account/Signout/v1');
    }
}

class Requests {
    public static get({
        type = 'Incoming',
        page = 1,
    }: {
        type?: 'Incoming' | 'Outgoing';
        page?: number;
    }) {
        return axios.get<{ friendRequests: FriendRequest[]; totalCount: number }>(`/API/Friends/Requests/v1?type=${type}&page=${page}`);
    }

    public static delete(id: string) {
        return axios.delete(`/API/Friends/Requests/${id}/v1`);
    }

    public static accept(id: string) {
        return axios.post(`/API/Friends/Requests/${id}/accept/v1`);
    }

    public static decline(id: string) {
        return axios.post(`/API/Friends/Requests/${id}/decline/v1`);
    }

    public static send(values: { recipientID: string; message?: string }) {
        return axios.post('/API/Friends/Requests/v1', values);
    }
}

class Friends {
    public static Requests = Requests;

    public static get() {
        return axios.get<Friend[]>('/API/Friends/v1');
    }

    public static remove(friendID: string) {
        return axios.post(`/API/Friends/${friendID}/remove/v1`);
    }
}

class Conversations {
    public static get() {
        return axios.get<ConversationsData>('/API/Conversations/v1');
    }

    public static leave(conversationID: string) {
        return axios.post(`/API/Conversations/${conversationID}/leave/v1`);
    }

    public static getOne(conversationID: string) {
        return axios.get<SingleConvoData>(`/API/Conversations/${conversationID}/v1`);
    }

    public static create(conversationName: string) {
        return axios.post<{ conversationID: string }>('/API/Conversations/v1', { conversationName });
    }

    public static join(conversationID: string) {
        return axios.post(`/API/Conversations/${conversationID}/join/v1`, { conversationID });
    }
}

export default class Endpoints {
    public static readonly Account = Account;
    public static readonly Friends = Friends;
    public static readonly Conversations = Conversations;
}
