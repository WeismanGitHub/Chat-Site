import { ToastContainer, Toast } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import Endpoints from '../../endpoints';

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

    async function leaveConvo() {
        try {
            await Endpoints.Conversations.leave(conversationID);
            window.location.reload();
        } catch (err: unknown) {
            setToastError(err as APIErrorRes<object>);
            setShowError(true);
            console.log(toastError);
        }
    }

    return (
        <>
            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowError(!showError)}
                    show={showError}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">
                            {toastError?.message || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {toastError?.errors &&
                            Object.values(toastError?.errors).map((err) => {
                                return <div key={err}>{err}</div>;
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            {conversation?.name}
            <div className="btn btn-outline-primary w-50 m-1" onClick={leaveConvo}>
                Leave
            </div>
        </>
    );
}
