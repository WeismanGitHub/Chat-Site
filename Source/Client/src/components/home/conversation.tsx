import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

type GetOneError = object;

export default function Conversation({
    conversationID,
    setConversations,
    conversations,
    setConvoID,
}: {
    setConvoID: Dispatch<SetStateAction<string | null>>;
    conversationID: string;
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
    conversations: ConversationsData;
}) {
    const { error, data } = useQuery<SingleConvoData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.getOne(conversationID),
    });

    const [toastError, setToastError] = useState<APIErrorRes<GetOneError> | null>(null);
    const [showError, setShowError] = useState(false);

    showError;
    toastError;
    conversations;

    async function leaveConvo() {
        try {
            await Endpoints.Conversations.leave(conversationID);
            setConversations(conversations.filter((convo) => convo.id !== conversationID));
            setConvoID(null);
        } catch (err: unknown) {
            if (err instanceof HTTPError) {
                setToastError(await err.response.json());
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setShowError(true);
                setToastError(res);
            });
        }
    }, [error]);

    return (
        <>
            {data?.name}
            <div className="btn btn-outline-primary w-50 m-1" onClick={leaveConvo}>
                Leave
            </div>
        </>
    );
}
