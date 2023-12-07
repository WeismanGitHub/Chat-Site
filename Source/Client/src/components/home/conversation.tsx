import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

type GetOneError = object;

export default function conversation({
    conversationID,
    setConversations,
}: {
    conversationID: string;
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
}) {
    const { error, data } = useQuery<SingleConvoData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.getOne(conversationID),
    });

    const [toastError, setToastError] = useState<APIErrorRes<GetOneError> | null>(null);
    const [showError, setShowError] = useState(false);

    data;
    showError;
    toastError;
    setConversations;

    // async function leaveConvo() {
    //     try {
    //         await Endpoints.Conversations.leave(selectedConvo!.id);
    //         data = data!.filter((convo) => convo.id === selectedConvo?.id);
    //         setConvo(null);
    //     } catch (err: unknown) {
    //         if (err instanceof HTTPError) {
    //             setToastError(await err.response.json());
    //             setShowError(true);
    //             console.log(toastError);
    //         }
    //     }
    // }

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setShowError(true);
                setToastError(res);
            });
        }
    }, [error]);

    return <>{data?.members}</>;
}
