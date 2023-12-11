import { useEffect, useState } from 'react';
import Endpoints from '../../../endpoints';
import { HTTPError } from 'ky';

type GetOneError = object;

export default function Conversation({ conversationID }: { conversationID: string }) {
    const [conversation, setConversation] = useState<SingleConvoData | null>(null);

    const [toastError, setToastError] = useState<APIErrorRes<GetOneError> | null>(null);
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        Endpoints.Conversations.getOne(conversationID)
            .then((res) => {
                setConversation(res);
            })
            .catch((error) => {
                console.error(error);
                setShowError(true);
                setToastError(error);
            });
    }, [conversationID]);

    showError;

    async function leaveConvo() {
        try {
            await Endpoints.Conversations.leave(conversationID);
            window.location.reload();
        } catch (err: unknown) {
            if (err instanceof HTTPError) {
                setToastError(await err.response.json());
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    return (
        <>
            {conversation?.name}
            <div className="btn btn-outline-primary w-50 m-1" onClick={leaveConvo}>
                Leave
            </div>
        </>
    );
}
