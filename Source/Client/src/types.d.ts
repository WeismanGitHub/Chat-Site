type APIError<Errors> = {
    errors: Errors;
    message: string;
    statusCode: number;
};

type Friend = {
    id: string;
    name: string;
    createdAt: string;
};

type Chats = {
    id: string;
    name: string;
    createdAt: string;
}[];

type Account = {
    id: string;
    name: string;
    chatRooms: number;
    createdAt: string;
};

type Member = { id: string; name: string };

type Chat = {
    id: string;
    name: string;
    createdAt: string;
    members: Member[];
};

type setState<T> = React.Dispatch<SetStateAction<T>>;
