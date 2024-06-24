type APIError<Errors> = {
    errors: Errors;
    message: string;
    statusCode: number;
};

type Friend = {
    id: string;
    displayName: string;
    createdAt: string;
};

type Chats = {
    id: string;
    name: string;
    createdAt: string;
}[];

type Account = {
    id: string;
    displayName: string;
    conversations: number;
    createdAt: string;
};

type Chat = {
    id: string;
    name: string;
    createdAt: string;
    members: { id: string; name: string }[];
};

type setState<T> = React.Dispatch<SetStateAction<T>>;
