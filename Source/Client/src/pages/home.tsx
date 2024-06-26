import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ListGroup, Row, Toast, ToastContainer } from 'react-bootstrap';
import { ReactNode, useEffect, useState } from 'react';
import { redirectIfNotLoggedIn } from '../helpers';
import Navbar from '../navbar';
import axios from 'axios';

export default function Home() {
    redirectIfNotLoggedIn();

    const [error, setError] = useState<APIError<unknown> | null>(null);
    const [chatID, setChatID] = useState<string | null>(null);

    return (
        <>
            <Navbar />
            <div className="full-height-minus-navbar">
                <div className="container-fluid p-0 m-0 w-100 h-100">
                    <Row className="h-100 w-100 p-0 m-0">
                        <Chats setChatID={setChatID} setError={setError} chatID={chatID} />
                        <Chat chatID={chatID} setError={setError} />
                    </Row>
                </div>
            </div>

            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setError(null)}
                    show={error !== null}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">{error?.message}</strong>
                    </Toast.Header>
                    <Toast.Body className="text-white">
                        <strong>{error?.errors?.toString() ?? 'Something went wrong.'}</strong>
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </>
    );
}

function Chats({
    setChatID,
    setError,
    chatID,
}: {
    setChatID: setState<string | null>;
    setError: setState<APIError<object>>;
    chatID: string | null;
}) {
    const [chats, setChats] = useState<Chats | null>(null);

    useEffect(() => {
        (async () => {
            try {
                const res = await axios.get<Chats>('/API/ChatRooms/v1');
                setChats(res.data);
            } catch (err) {
                if (axios.isAxiosError<APIError<object>>(err) && err.response?.data) {
                    setError({
                        errors: err.response.data.errors ?? [],
                        statusCode: err.response.status,
                        message: err.response.data.message ?? 'Something went wrong!',
                    });
                } else {
                    setError({
                        errors: [],
                        statusCode: 500,
                        message: 'Something went wrong!',
                    });
                }
            }
        })();
    }, []);

    return (
        <>
            <button
                className="btn btn-primary d-md-none ms-1"
                type="button"
                data-bs-toggle="offcanvas"
                data-bs-target="#offcanvasResponsive"
                aria-controls="offcanvasResponsive"
                style={{ position: 'absolute', top: '50%', left: 0, width: '40px' }}
            >
                {'>'}
            </button>

            <div
                className="offcanvas offcanvas-start bg-primary-subtle p-0"
                tabIndex={-1}
                style={{ maxWidth: '85%' }}
                id="offcanvasResponsive"
                aria-labelledby="offcanvasResponsiveLabel"
            >
                <div className="offcanvas-header" style={{ backgroundColor: '#593196', color: 'white' }}>
                    <h5 className="offcanvas-title" id="offcanvasResponsiveLabel">
                        Chat Rooms
                    </h5>
                    <button
                        type="button"
                        className="btn-close custom-close-btn"
                        data-bs-dismiss="offcanvas"
                        data-bs-target="#offcanvasResponsive"
                        aria-label="Close"
                    ></button>
                </div>
                <div className="offcanvas-body w-100" style={{ backgroundColor: '#7756b0' }}>
                    <ChatsList />
                </div>
            </div>

            <div
                className="col-2 d-none d-md-block p-0 m-0"
                style={{ height: '100px', position: 'absolute', top: 0, left: 0, backgroundColor: '#7756b0' }}
            />
            <div className="col-2 d-none d-md-block h-100 fs-5 p-0 m-0">
                <ChatsList />
            </div>
        </>
    );

    function ChatsList() {
        return (
            <div className="h-100" style={{ backgroundColor: '#7756b0', color: 'white' }}>
                {!chats?.length ? (
                    <div className="w-100 h-100 d-flex justify-content-center align-items-center">
                        <strong style={{ height: '50px' }}>No Chats</strong>
                    </div>
                ) : (
                    <ListGroup variant="flush" className="custom-list-group">
                        {chats.map((chat) => (
                            <ListGroup.Item
                                className="custom-list-item"
                                active={chatID == chat.id}
                                key={chat.id}
                                action
                                onClick={() => setChatID(chat.id)}
                            >
                                {chat.name}
                            </ListGroup.Item>
                        ))}
                    </ListGroup>
                )}
            </div>
        );
    }
}

function Chat({ chatID, setError }: { chatID: string | null; setError: setState<APIError<unknown> | null> }) {
    const [connection, setConnection] = useState<null | HubConnection>(null);
    const [messages, setMessages] = useState<ReactNode[]>([]);
    const [chat, setChat] = useState<Chat | null>(null);
    const [input, setInput] = useState('');

    const memberMap = new Map<string, string>();

    useEffect(() => {
        if (!chatID) return;

        axios
            .get<Chat>(`/API/ChatRooms/${chatID}/v1`)
            .then((res) => {
                setChat(res.data);

                memberMap.clear();
                res.data.members.forEach((member) => {
                    memberMap.set(member.id, member.name);
                });
            })
            .catch(() =>
                setError({
                    errors: {},
                    message: 'Could not get chat room data.',
                    status: 0,
                })
            );

        const connect = new HubConnectionBuilder()
            .withUrl(`/chat?chatRoomID=${chatID}`)
            .withAutomaticReconnect()
            .build();

        setConnection(connect);

        return () => {
            connection?.stop();
        };
    }, [chatID]);

    useEffect(() => {
        if (!connection) return;

        connection
            .start()
            .then(() => {
                connection.on(
                    'ReceiveMessage',
                    ({ accountID, message }: { accountID: string; message: string }) => {
                        const name = memberMap.get(accountID) ?? 'Unknown';

                        setMessages((prevMessages) => [
                            ...prevMessages,
                            <div key={Date.now() + accountID + Math.random()}>
                                {name} - {message}
                            </div>,
                        ]);
                    }
                );

                connection.on('ReceiveError', (err: string) => {
                    setError({
                        errors: {},
                        message: err,
                        statusCode: 500,
                    });
                });

                connection.on('UserConnected', (id) => {
                    const name = memberMap.get(id) ?? 'Unknown';

                    setMessages((prevMessages) => [
                        ...prevMessages,
                        <div key={Date.now() + id} className="text-danger">
                            {name} Left!
                        </div>,
                    ]);
                });

                connection.on('UserDisconnected', (id) => {
                    const name = memberMap.get(id) ?? 'Unknown';

                    setMessages((prevMessages) => [
                        ...prevMessages,
                        <div key={Date.now() + id} className="text-success">
                            {name} Joined!
                        </div>,
                    ]);
                });

                connection.on('UserLeft', (member: Member) => {
                    console.log(member);
                });

                connection.on('UserJoined', (userID: string) => {
                    memberMap.delete(userID);
                    console.log(userID);
                });
            })
            .catch((error: Error) => {
                setError({
                    errors: {},
                    message: error.message,
                    statusCode: 500,
                });
            });
    }, [connection]);

    async function sendMessage() {
        if (input.length > 1000 || input.length === 0) {
            return setError({
                errors: {},
                message: 'Invalid Input Length',
                statusCode: 400,
            });
        }

        if (!connection) return;

        await connection.send('SendMessage', input);
        setInput('');
    }

    console.log(sendMessage, messages, chat);

    //     return (
    //         <>

    //             <div className="d-flex flex-column" style={{ height: '600px' }}>
    //                 <div className="overflow-y-scroll text-wrap float-start" style={{ minHeight: '550px' }}>
    //                     {messages}
    //                 </div>
    //                 <div className="d-flex p-2">
    //                     <input
    //                         className="form-control rounded me-1"
    //                         type="text"
    //                         value={input}
    //                         placeholder=" ..."
    //                         onChange={(event) => {
    //                             setInput(event.target.value);
    //                         }}
    //                         onKeyPress={(event) => {
    //                             event.key === 'Enter' && sendMessage();
    //                         }}
    //                     />
    //                     <button className="btn btn-primary rounded" onClick={sendMessage}>
    //                         Send
    //                     </button>
    //                 </div>
    //             </div>
    //         </>
    //     );
    // }

    return !chat ? (
        <div className="m-0 p-0 col-sm-12 col-md-10 d-flex justify-content-center align-items-center h-100">
            <h2>No Chat Selected</h2>
        </div>
    ) : (
        <>
            <div className="col-md-8 col-sm-12 h-100">Chat Selected</div>
            <>
                <button
                    className="btn btn-primary d-md-none me-1"
                    type="button"
                    data-bs-toggle="offcanvas"
                    data-bs-target="#offcanvasExample"
                    aria-controls="offcanvasExample"
                    style={{ position: 'absolute', top: '50%', right: 0, width: '40px' }}
                >
                    {'<'}
                </button>

                <div
                    className="offcanvas offcanvas-end bg-primary-subtle p-0"
                    tabIndex={-1}
                    style={{ maxWidth: '85%' }}
                    id="offcanvasExample"
                    aria-labelledby="offcanvasExampleLabel"
                >
                    <div className="offcanvas-header" style={{ backgroundColor: '#593196', color: 'white' }}>
                        <h5 className="offcanvas-title" id="offcanvasExampleLabel">
                            Chat Rooms
                        </h5>
                        <button
                            type="button"
                            className="btn-close custom-close-btn"
                            data-bs-dismiss="offcanvas"
                            data-bs-target="#offcanvasExample"
                            aria-label="Close"
                        ></button>
                    </div>
                    <div className="offcanvas-body w-100" style={{ backgroundColor: '#7756b0' }}>
                        <MembersList />
                    </div>
                </div>

                <div
                    className="col-2 d-none d-md-block p-0 m-0"
                    style={{
                        height: '100px',
                        position: 'absolute',
                        top: 0,
                        right: 0,
                        backgroundColor: '#7756b0',
                    }}
                />
                <div className="col-2 d-none d-md-block fs-5 float-end p-0 m-0 h-100">
                    <MembersList />
                </div>
            </>
        </>
    );

    function MembersList() {
        return (
            <div className="h-100" style={{ backgroundColor: '#7756b0', color: 'white' }}>
                <ListGroup variant="flush" className="custom-list-group">
                    {chat?.members.map((member) => (
                        <ListGroup.Item key={member.id} className="custom-list-item">
                            {member.name}
                        </ListGroup.Item>
                    ))}
                </ListGroup>
            </div>
        );
    }
}

// function CreateConvo() {
//     const schema = yup.object().shape({
//         conversationID: yup
//             .string()
//             .required('Convo ID is a required field.')
//             .min(1, 'Must be at least 1 characters.')
//             .max(25, 'Cannot be more than 25 characters.'),
//     });

//     async function joinConvo(values: { conversationID: string }) {
//         try {
//             await Endpoints.Conversations.join(values.conversationID);
//             window.location.reload();
//             setShowModal(false);
//         } catch (err: unknown) {
//             if (axios.isAxiosError<APIError<object>>(err) && err.response?.data) {
//                 setToastError(err.response.data);
//                 setShowError(true);
//                 console.log(toastError);
//             }
//         }
//     }

//     const [toastError, setToastError] = useState<APIError<object> | null>(null);
//     const [showError, setShowError] = useState(false);
//     const [showModal, setShowModal] = useState(false);

//     return (
//         <>
//             <div className="btn btn-outline-primary" onClick={() => setShowModal(true)}>
//                 Join Convo
//             </div>

//             <ToastContainer position="top-end">
//                 <Toast
//                     onClose={() => setShowError(false)}
//                     show={showError}
//                     autohide={true}
//                     className="d-inline-block m-1"
//                     bg={'danger'}
//                 >
//                     <Toast.Header>
//                         <strong className="me-auto">
//                             {toastError?.message || 'Unable to read error name.'}
//                         </strong>
//                     </Toast.Header>
//                     <Toast.Body>
//                         {toastError?.errors &&
//                             Object.values(toastError?.errors).map((err) => {
//                                 return <div key={err}>{err}</div>;
//                             })}
//                     </Toast.Body>
//                 </Toast>
//             </ToastContainer>

//             <Modal show={showModal}>
//                 <Modal.Dialog>
//                     <Modal.Header closeButton onClick={() => setShowModal(false)}></Modal.Header>
//                     <Modal.Body>
//                         <div className="w-100">
//                             <Formik
//                                 validationSchema={schema}
//                                 validateOnChange
//                                 onSubmit={joinConvo}
//                                 initialValues={{ conversationID: '' }}
//                             >
//                                 {({ handleSubmit, handleChange, values, errors }) => (
//                                     <Form noValidate onSubmit={handleSubmit}>
//                                         <Row className="mb-3">
//                                             <Form.Group as={Col} controlId="conversationID">
//                                                 <Form.Label>Convo ID</Form.Label>
//                                                 <Form.Control
//                                                     type="text"
//                                                     name="conversationID"
//                                                     value={values.conversationID}
//                                                     onChange={handleChange}
//                                                     isInvalid={!!errors.conversationID}
//                                                 />
//                                                 <Form.Control.Feedback type="invalid">
//                                                     {errors.conversationID}
//                                                 </Form.Control.Feedback>
//                                             </Form.Group>
//                                         </Row>
//                                         <Button type="submit">Join</Button>
//                                     </Form>
//                                 )}
//                             </Formik>
//                         </div>
//                     </Modal.Body>
//                 </Modal.Dialog>
//             </Modal>
//         </>
//     );
// }
