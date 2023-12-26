import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNode, useEffect, useState } from 'react';

export default function Chat({ conversationID, members }: { conversationID: string, members: { id: string, name: string }[] }) {
    const [connection, setConnection] = useState<null | HubConnection>(null);
    const [messages, setMessages] = useState<ReactNode[]>([])
    const memberMap = new Map<string, string>()
    const [input, setInput] = useState('');

    members.forEach(member => {
        memberMap.set(member.id, member.name)
    })

    conversationID

    useEffect(() => {
        const connect = new HubConnectionBuilder().withUrl('/chat').withAutomaticReconnect().build();

        setConnection(connect);
    }, []);

    useEffect(() => {
        if (!connection) {
            return;
        }

        connection
            .start()
            .then(() => {
                connection.on('ReceiveMessage', ({ id, message }: { id: string, message: string }) => {
                    const name = memberMap.get(id) ?? "Unknown"

                    setMessages([...messages, <div key={id+message}>
                        {name} - {message}
                    </div>])
                });

                connection.on('UserLeft', (message) => {
                    console.log(message);
                });

                connection.on('UserJoined', (message) => {
                    console.log(message);
                });
            })
            .catch((error) => console.log(error));
    }, [connection]);

    async function sendMessage() {
        if (input.length > 1000 || input.length === 0) {
            return;
        }

        if (!connection) return;

        await connection.send('SendMessage', input);
        setInput('')
    }

    return (
        <>
            <div className="">
                You joined!
                {messages}
            </div>
            <input
                type="text"
                value={input}
                placeholder="..."
                onChange={(event) => {
                    setInput(event.target.value);
                }}
                onKeyPress={(event) => {
                    event.key === 'Enter' && sendMessage();
                }}
            />
            <button onClick={sendMessage}>Send</button>
        </>
    );
}
