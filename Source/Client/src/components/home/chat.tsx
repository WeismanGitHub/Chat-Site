import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Toast, ToastContainer } from 'react-bootstrap';
import { ReactNode, useEffect, useState } from 'react';

export default function Chat({
    conversationID,
    members,
}: {
    conversationID: string;
    members: { id: string; name: string }[];
}) {
    const [connection, setConnection] = useState<null | HubConnection>(null);
    const [messages, setMessages] = useState<ReactNode[]>([]);
    const memberMap = new Map<string, string>();
    const [input, setInput] = useState('');

    members.forEach((member) => {
        memberMap.set(member.id, member.name);
    });

    const [error, setError] = useState<string | null>(null);
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        const connect = new HubConnectionBuilder()
            .withUrl(`/chat?conversationID=${conversationID}`)
            .withAutomaticReconnect()
            .build();

        setConnection(connect);

        return () => {
            connection?.stop();
        };
    }, []);

    useEffect(() => {
        if (!connection) {
            return;
        }

        connection
            .start()
            .then(() => {
                connection.on(
                    'ReceiveMessage',
                    ({ accountID, message }: { accountID: string; message: string }) => {
                        const name = memberMap.get(accountID) ?? 'Unknown';

                        setMessages((prevMessages) => [
                            ...prevMessages,
                            <div key={Date.now() + accountID}>
                                {name} - {message}
                            </div>,
                        ]);
                    }
                );

                connection.on('ReceiveError', (err: string) => {
                    setError(err);
                    setShowError(true);
                });

                connection.on('UserLeft', (id) => {
                    const name = memberMap.get(id) ?? 'Unknown';

                    setMessages((prevMessages) => [
                        ...prevMessages,
                        <div key={Date.now() + id} className="text-danger">
                            {name} Left!
                        </div>,
                    ]);
                });

                connection.on('UserJoined', (id) => {
                    const name = memberMap.get(id) ?? 'Unknown';

                    setMessages((prevMessages) => [
                        ...prevMessages,
                        <div key={Date.now() + id} className="text-success">
                            {name} Joined!
                        </div>,
                    ]);
                });
            })
            .catch((error: Error) => {
                setError(error?.message);
                setShowError(true);
            });
    }, [connection]);

    async function sendMessage() {
        if (input.length > 1000 || input.length === 0) {
            setError('Message must be between 1,000 and 0 characters.');
            setShowError(true);
            return;
        }

        if (!connection) return;

        await connection.send('SendMessage', input);
        setInput('');
    }

    return (
        <>
            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowError(false)}
                    show={showError}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">An error occured!</strong>
                    </Toast.Header>
                    <Toast.Body>{error}</Toast.Body>
                </Toast>
            </ToastContainer>

            <div className="d-flex flex-column" style={{ height: '600px' }}>
                <div className="overflow-y-scroll text-wrap float-start" style={{ minHeight: '550px' }}>
                    {messages}
                </div>
                <div className="d-flex p-2">
                    <input
                        className="form-control rounded me-1"
                        type="text"
                        value={input}
                        placeholder=" ..."
                        onChange={(event) => {
                            setInput(event.target.value);
                        }}
                        onKeyPress={(event) => {
                            event.key === 'Enter' && sendMessage();
                        }}
                    />
                    <button className="btn btn-primary rounded" onClick={sendMessage}>
                        Send
                    </button>
                </div>
            </div>
        </>
    );
}
