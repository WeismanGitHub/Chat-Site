import { ToastContainer, Toast, Modal, Button } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

export default function Conversations() {
    // eslint-disable-next-line prefer-const
    let { error, data } = useQuery<ConversationsData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.get(),
    });

    const [toastError, setToastError] = useState<APIErrorRes<object> | null>(null);
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setToastError(res);
                setShowError(true);
            });
        }
    }, [error]);

    const [showModal, setShowModal] = useState(false);
    const [selectedConvo, setConvo] = useState<{
        id: string;
        name: string;
        createdAt: string;
    } | null>(null);

    async function leaveConvo() {
        try {
            await Endpoints.Conversations.leave(selectedConvo!.id);
            data = data!.filter((convo) => convo.id === selectedConvo?.id);
            setConvo(null);
        } catch (err: unknown) {
            if (err instanceof HTTPError) {
                setToastError(await err.response.json());
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    return (
        <div className="text-center">
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

            <Modal show={showModal}>
                <Modal.Dialog>
                    <Modal.Header
                        closeButton
                        onClick={() => {
                            setShowModal(false);
                            setConvo(null);
                        }}
                    >
                        <Modal.Title>Conversation</Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        {selectedConvo?.name || "Could not get friend's name."}
                        <div className="fs-6">
                            Created -{' '}
                            {!selectedConvo
                                ? 'Unkown'
                                : new Date(selectedConvo.createdAt).toLocaleDateString('en-US', {
                                      weekday: 'long',
                                      year: 'numeric',
                                      month: 'long',
                                      day: 'numeric',
                                  })}
                        </div>
                    </Modal.Body>

                    <Modal.Footer>
                        <Button variant="danger" onClick={leaveConvo}>
                            Remove
                        </Button>
                    </Modal.Footer>
                </Modal.Dialog>
            </Modal>

            <ul className="list-group fs-5">
                {data?.map((convo) => {
                    return (
                        <li
                            className="list-group-item bg-dark-subtle text-primary border-secondary"
                            key={convo.id}
                            onClick={() => {
                                setConvo(convo);
                                setShowModal(true);
                            }}
                        >
                            {convo.name}
                            <div className="fs-6">
                                Created -{' '}
                                {new Date(convo.createdAt).toLocaleDateString('en-US', {
                                    weekday: 'long',
                                    year: 'numeric',
                                    month: 'long',
                                    day: 'numeric',
                                })}
                            </div>
                        </li>
                    );
                })}
            </ul>
        </div>
    );
}
