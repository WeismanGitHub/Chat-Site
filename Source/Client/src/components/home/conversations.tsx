import { ToastContainer, Toast, Modal, Button } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../../endpoints';
import { useState } from 'react';
import { HTTPError } from 'ky';

export default function Conversations() {
    let { error, data } = useQuery<ConversationsData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.get(),
    });

    const [showError, setShowError] = useState(false);

    if (error) {
        setShowError(true);
    }

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
                error = err;
                setShowError(true);
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
                        <strong className="me-auto">{error?.name || 'Unable to read error name.'}</strong>
                    </Toast.Header>
                    <Toast.Body>{error?.message || 'Unable to read error message.'}</Toast.Body>
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
