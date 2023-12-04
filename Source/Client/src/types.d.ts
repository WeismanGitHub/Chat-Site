type APIErrorRes<APIError> = {
    errors: APIError & { generalErrors?: string };
    message: string;
    statusCode: number;
};

type Friend = {
    id: string;
    displayName: string;
    createdAt: string;
};

type Conversation = {
    id: string;
    name: string;
    createdAt: string;
};

type AccountData = {
    id: string;
    displayName: string;
    email: string;
    totalConversations: number;
    totalFriends: number;
    createdAt: string;
};
