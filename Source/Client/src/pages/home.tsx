import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNode, useEffect, useRef, useState } from 'react';
import { redirectIfNotLoggedIn } from '../helpers';
import { useNavigate } from 'react-router-dom';
import { Formik } from 'formik';
import Navbar from '../navbar';
import * as yup from 'yup';
import axios from 'axios';
import {
    Button,
    Col,
    Form,
    FormControl,
    FormGroup,
    FormLabel,
    InputGroup,
    ListGroup,
    Modal,
    Row,
    Toast,
    ToastContainer,
} from 'react-bootstrap';

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
    const [chats, setChats] = useState<Chats>([]);
    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            try {
                const res = await axios.get<Chats>('/API/ChatRooms/v1');

                if (!Array.isArray(res.data)) {
                    navigate('/auth');
                    localStorage.removeItem('authenticated');
                }

                setChats(res.data);
            } catch (err) {
                if (axios.isAxiosError<APIError<object>>(err) && err.response?.data) {
                    if (err.status === 401) {
                        localStorage.removeItem('authenticated');
                        navigate('/auth');
                    }

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
                className="btn btn-primary d-md-none ms-1 p-1"
                type="button"
                data-bs-toggle="offcanvas"
                data-bs-target="#offcanvasResponsive"
                aria-controls="offcanvasResponsive"
                style={{ position: 'absolute', top: '50%', left: 0, width: 'fit-content' }}
            >
                {'>'}
            </button>

            <div
                className="offcanvas offcanvas-start bg-primary-subtle p-0"
                tabIndex={-1}
                style={{ maxWidth: '50%' }}
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
                style={{ height: '80px', position: 'absolute', top: 0, left: 0, backgroundColor: '#7756b0' }}
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
                        <ChatsButtons />
                    </div>
                ) : (
                    <>
                        <div className="pb-2" style={{ borderBottom: '1.5px solid #513d76' }}>
                            <ChatsButtons />
                        </div>
                        <ListGroup variant="flush" className="custom-list-group">
                            {Array.isArray(chats) &&
                                chats?.map((chat) => (
                                    <ListGroup.Item
                                        className="custom-list-item"
                                        active={chatID == chat.id}
                                        key={chat.id}
                                        action
                                        onClick={() => setChatID(chat.id)}
                                    >
                                        <Row>
                                            <Col className="flex-column flex-grow-1">{chat.name}</Col>
                                            <Col
                                                className="p-0 m-0"
                                                xs="auto"
                                                style={{ width: '22.5px' }}
                                                onClick={(e) => e.stopPropagation()}
                                            >
                                                <svg
                                                    data-bs-toggle="dropdown"
                                                    xmlns="http://www.w3.org/2000/svg"
                                                    fill="white"
                                                    className="bi bi-three-dots-vertical float-end"
                                                    viewBox="0 0 16 16"
                                                    width="22.5"
                                                    height="22.5"
                                                >
                                                    <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
                                                </svg>
                                                <div className="dropdown-menu dropdown-menu-start">
                                                    <div
                                                        className="dropdown-item"
                                                        onClick={() => navigator.clipboard.writeText(chat.id)}
                                                    >
                                                        Copy ID
                                                    </div>
                                                    <div
                                                        className="dropdown-item"
                                                        onClick={async () => {
                                                            try {
                                                                await axios.post(
                                                                    `/API/ChatRooms/${chat.id}/Leave/v1`
                                                                );
                                                                setChats(
                                                                    chats.filter((c) => c.id !== chat.id)
                                                                );

                                                                if (chatID === chat.id) {
                                                                    setChatID(null);
                                                                }
                                                            } catch (err) {
                                                                setError({
                                                                    errors: {},
                                                                    message: 'Could not leave chat!',
                                                                    statusCode: 500,
                                                                });
                                                            }
                                                        }}
                                                    >
                                                        Leave
                                                    </div>
                                                </div>
                                            </Col>
                                        </Row>
                                    </ListGroup.Item>
                                ))}
                        </ListGroup>
                    </>
                )}
            </div>
        );
    }

    function ChatsButtons() {
        const [showCreate, setShowCreate] = useState(false);
        const [showJoin, setShowJoin] = useState(false);

        return (
            <>
                <Row className="d-flex justify-content-center">
                    <Button
                        className="m-1 btn-md"
                        onClick={() => setShowCreate(true)}
                        style={{ width: 'fit-content', minWidth: '72px' }}
                    >
                        Create
                    </Button>
                    <Button
                        className="m-1 btn-md"
                        style={{ width: 'fit-content', minWidth: '72px' }}
                        onClick={() => setShowJoin(true)}
                    >
                        Join
                    </Button>
                </Row>

                <Formik
                    validationSchema={yup.object({
                        name: yup
                            .string()
                            .required('Name is required.')
                            .max(25, 'Name cannot be more than 25 characters.'),
                    })}
                    validateOnChange
                    onSubmit={async (values) => {
                        try {
                            const res = await axios.post<{ id: string }>('/API/ChatRooms/v1', {
                                name: values.name,
                            });

                            setChats([
                                ...(chats ?? []),
                                { createdAt: new Date().toString(), id: res.data.id, name: values.name },
                            ]);

                            setShowCreate(false);
                        } catch (err) {
                            if (axios.isAxiosError<APIError<unknown>>(err) && err.response?.data) {
                                setError(err.response.data);
                            }
                        }
                    }}
                    initialValues={{
                        name: '',
                    }}
                >
                    {({ handleSubmit, handleChange, values, errors }) => (
                        <Modal
                            show={showCreate}
                            centered
                            keyboard={true}
                            onHide={() => setShowCreate(false)}
                            animation={false}
                        >
                            <Form noValidate onSubmit={handleSubmit}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Create</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="name">
                                            <FormLabel>Name</FormLabel>
                                            <InputGroup hasValidation>
                                                <FormControl
                                                    autoComplete="off"
                                                    autoFocus
                                                    aria-describedby="inputGroupPrepend"
                                                    name="name"
                                                    value={values.name}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.name}
                                                />
                                                <FormControl.Feedback type="invalid">
                                                    {errors.name}
                                                </FormControl.Feedback>
                                            </InputGroup>
                                        </FormGroup>
                                    </Row>
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button type="submit">Create</Button>
                                    <Button variant="secondary" onSubmit={() => setShowCreate(false)}>
                                        Close
                                    </Button>
                                </Modal.Footer>
                            </Form>
                        </Modal>
                    )}
                </Formik>

                <Formik
                    validationSchema={yup.object({
                        id: yup
                            .string()
                            .required('ID is required.')
                            .max(30, 'ID cannot be more than 30 characters.'),
                    })}
                    validateOnChange
                    onSubmit={async (values) => {
                        try {
                            const res = await axios.post<{ name: string; createdAt: Date }>(
                                `/API/ChatRooms/${values.id}/Join/v1`
                            );

                            setChats([
                                ...(chats ?? []),
                                {
                                    createdAt: res.data.createdAt.toString(),
                                    id: values.id,
                                    name: res.data.name,
                                },
                            ]);

                            setShowCreate(false);
                        } catch (err) {
                            if (axios.isAxiosError<APIError<unknown>>(err) && err.response?.data) {
                                setError(err.response.data);
                            }
                        }
                    }}
                    initialValues={{
                        id: '',
                    }}
                >
                    {({ handleSubmit, handleChange, values, errors }) => (
                        <Modal
                            show={showJoin}
                            centered
                            keyboard={true}
                            onHide={() => setShowJoin(false)}
                            animation={false}
                        >
                            <Form noValidate onSubmit={handleSubmit}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Join</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="idid">
                                            <FormLabel>ID</FormLabel>
                                            <InputGroup hasValidation>
                                                <FormControl
                                                    autoComplete="off"
                                                    autoFocus
                                                    aria-describedby="inputGroupPrepend"
                                                    name="id"
                                                    value={values.id}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.id}
                                                />
                                                <FormControl.Feedback type="invalid">
                                                    {errors.id}
                                                </FormControl.Feedback>
                                            </InputGroup>
                                        </FormGroup>
                                    </Row>
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button type="submit">Join</Button>
                                    <Button variant="secondary" onSubmit={() => setShowJoin(false)}>
                                        Close
                                    </Button>
                                </Modal.Footer>
                            </Form>
                        </Modal>
                    )}
                </Formik>
            </>
        );
    }
}

function Chat({ chatID, setError }: { chatID: string | null; setError: setState<APIError<unknown> | null> }) {
    const [connection, setConnection] = useState<null | HubConnection>(null);
    const [messages, setMessages] = useState<ReactNode[]>([]);
    const [chat, setChat] = useState<Chat | null>(null);
    const [input, setInput] = useState('');
    const messageEnd = useRef(null);

    useEffect(() => {
        // @ts-expect-error nothing
        messageEnd.current?.scrollIntoView({ behavior: 'instant' });
    }, [messages]);

    const memberMap = new Map<string, string>();

    useEffect(() => {
        (async () => {
            if (!chatID && chat !== null) return setChat(null);
            if (!chatID) return;
            setMessages([]);

            try {
                const res = await axios.get<Chat>(`/API/ChatRooms/${chatID}/v1`);
                setChat(res.data);

                memberMap.clear();
                res.data.members.forEach((member) => {
                    memberMap.set(member.id, member.name);
                });

                const connect = new HubConnectionBuilder()
                    .withUrl(`/chat?chatRoomID=${chatID}`)
                    .withAutomaticReconnect()
                    .build();

                setConnection(connect);
                connect
                    .start()
                    .then(() => {
                        connect.on(
                            'ReceiveMessage',
                            ({ accountID, message }: { accountID: string; message: string }) => {
                                const name = memberMap.get(accountID) ?? 'Unknown';

                                setMessages((prevMessages) => [
                                    ...prevMessages,
                                    <li
                                        className="p-2 rounded mb-1 bg-info"
                                        style={{ color: 'white', width: 'fit-content' }}
                                        key={Date.now() + accountID + Math.random()}
                                    >
                                        <strong>{name}</strong>: {message}
                                    </li>,
                                ]);
                            }
                        );

                        connect.on('ReceiveError', (err: string) => {
                            setError({
                                errors: {},
                                message: err,
                                statusCode: 500,
                            });
                        });

                        connect.on('UserConnected', (id) => {
                            const name = memberMap.get(id) ?? 'Unknown';

                            setMessages((prevMessages) => [
                                ...prevMessages,
                                <li
                                    className="p-2 bg-success rounded mb-1"
                                    style={{ color: 'white', width: 'fit-content' }}
                                    key={id + Date.now()}
                                >
                                    <strong>{name}</strong>: Connected
                                </li>,
                            ]);
                        });

                        connect.on('UserDisconnected', (id) => {
                            const name = memberMap.get(id) ?? 'Unknown';

                            setMessages((prevMessages) => [
                                ...prevMessages,
                                <li
                                    className="p-2 bg-danger rounded mb-1"
                                    style={{ color: 'white', width: 'fit-content' }}
                                    key={id + Date.now()}
                                >
                                    <strong>{name}</strong>: Disconnected
                                </li>,
                            ]);
                        });

                        connect.on('UserJoined', (member: Member) => {
                            memberMap.set(member.id, member.name);

                            setMessages((prevMessages) => [
                                ...prevMessages,
                                <li
                                    className="p-2 bg-success rounded mb-1"
                                    style={{ color: 'white', width: '100%' }}
                                    key={member.id + Date.now()}
                                >
                                    <strong>{member.name}</strong>: Joined
                                </li>,
                            ]);
                        });

                        connect.on('UserLeft', (userID: string) => {
                            setMessages((prevMessages) => [
                                ...prevMessages,
                                <li
                                    className="p-2 bg-danger rounded mb-1"
                                    style={{ color: 'white', width: '100%' }}
                                    key={userID + Date.now()}
                                >
                                    <strong>{memberMap.get(userID)}</strong>: Left
                                </li>,
                            ]);
                        });
                    })
                    .catch((error: Error) => {
                        setError({
                            errors: {},
                            message: error.message,
                            statusCode: 500,
                        });
                    });
            } catch (err) {
                setError({
                    errors: {},
                    message: 'Could not get chat room data.',
                    status: 0,
                });
            }
        })();

        return () => {
            connection?.stop();
        };
    }, [chatID]);

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

    return !chat ? (
        <div className="m-0 p-0 col-sm-12 col-md-10 d-flex justify-content-center align-items-center h-100">
            <h2>No Chat Selected</h2>
        </div>
    ) : (
        <>
            <div className="col-md-8 col-sm-12 h-100 p-0 m-0 d-flex flex-column">
                <ul className="overflow-y-auto text-wrap w-100 text-break flex-grow-1 ps-2 pe-2">
                    {messages}
                    <li ref={messageEnd}></li>
                </ul>
                <Row className="d-flex justify-content-center ps-2 pe-2 m-0 mb-1">
                    <input
                        type="text"
                        className="form-control rounded m-0"
                        style={{ width: '85%' }}
                        value={input}
                        placeholder="..."
                        onChange={(event) => {
                            setInput(event.target.value);
                        }}
                        onKeyPress={(event) => {
                            if (!input.length) return;

                            event.key === 'Enter' && sendMessage();
                        }}
                    />
                    <button
                        type="button"
                        className="btn-primary btn-lg btn ms-1 p-1"
                        onClick={sendMessage}
                        style={{ width: 'fit-content' }}
                    >
                        Send
                    </button>
                </Row>
            </div>

            <button
                className="btn btn-primary d-md-none me-1 p-1"
                type="button"
                data-bs-toggle="offcanvas"
                data-bs-target="#offcanvasExample"
                aria-controls="offcanvasExample"
                style={{ position: 'absolute', top: '50%', right: 0, width: 'fit-content' }}
            >
                {'<'}
            </button>

            <div
                className="offcanvas offcanvas-end bg-primary-subtle p-0"
                tabIndex={-1}
                style={{ maxWidth: '50%' }}
                id="offcanvasExample"
                aria-labelledby="offcanvasExampleLabel"
            >
                <div className="offcanvas-header" style={{ backgroundColor: '#593196', color: 'white' }}>
                    <h5 className="offcanvas-title" id="offcanvasExampleLabel">
                        Members
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
