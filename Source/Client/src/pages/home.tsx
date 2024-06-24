import { redirectIfNotLoggedIn } from '../helpers';
import { useEffect, useState } from 'react';
import Navbar from '../navbar';
import axios from 'axios';
import { Toast, ToastContainer } from 'react-bootstrap';

export default function Home() {
    redirectIfNotLoggedIn();

    const [chatID, setChatID] = useState<string | null>(null);
    const [error, setError] = useState<APIError<unknown> | null>(null)

    return (
        <>
            <Navbar />
            <div className="full-height-minus-navbar">
                <div className="col-3">
                    {<Chats setChatID={setChatID} setError={setError}/>}
                </div>
                <div className="col-9">
                    <Chat chatID={chatID}/>
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

function Chats({ setChatID, setError }: { setChatID: setState<string | null>; setError: setState<APIError<object>> }) {
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
                        message: err.response.data.message ?? 'Something went wrong!'
                    })
                } else {
                    setError({
                        errors: [],
                        statusCode: 500,
                        message: 'Something went wrong!'
                    })
                }
            }
        })();
    }, [])

    console.log(chats, setChatID)

    return <>
    {/* useEffect(() => {
        (async () => {
            try {
                const res = await axios.patch<Chats>(`/API/ChatRooms/${chatID}/v1`);

                setChat(res.data)
    
                setAccount({
                    id: account.id,
                    conversations: account.conversations,
                    createdAt: account.createdAt,
                    email: update.email ?? account.email,
                    displayName: update.displayName ?? account.displayName
                })
    
                setShowUpdateModal(false);
            } catch (err) {
                if (axios.isAxiosError<APIError<UpdateError>>(err) && err.response?.data) {
                    setToastError(err.response.data);
                    setShowError(true);
                }
            }
        })()
        Endpoints.Conversations.getOne(conversationID)
            .then((res) => {
                setChat(res.data);
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
            setToastError(err as APIError<object>);
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

            <div className="fs-2 justify-content-center">
                {chat?.name}
                <div
                    className="btn btn-outline-primary m-1 ms-5 fs-6 p-0"
                    onClick={leaveConvo}
                    style={{ width: '10%' }}
                >
                    Leave
                </div>
                <div
                    className="btn btn-outline-primary m-1 fs-6 p-0"
                    onClick={() => {
                        navigator.clipboard.writeText(conversationID);
                    }}
                    style={{ width: '10%' }}
                >
                    Copy ID
                </div>
            </div>
            <div className="overflow-y-scroll h-25 vh-100 pb-5 col-3 float-end">
                <ul className="list-group fs-5">
                    {chat &&
                        chat.members.map((member) => {
                            return <Member id={member.id} name={member.name} key={member.id} />;
                        })}
                    <br />
                    <br />
                    <br />
                    <br />
                </ul>
            </div>

            <div className="col-9 float-start">
                {chat?.members.length && (
                    <Chat conversationID={conversationID} members={chat?.members} />
                )}
            </div>
        </>
    );
} */}

    </>
}

function Chat({ chatID }: { chatID: string | null }) {
    console.log(chatID)
    return <></>
}

// function Chat({
//     conversationID,
//     members,
// }: {
//     conversationID: string;
//     members: { id: string; name: string }[];
// }) {
//     const [connection, setConnection] = useState<null | HubConnection>(null);
//     const [messages, setMessages] = useState<ReactNode[]>([]);
//     const memberMap = new Map<string, string>();
//     const [input, setInput] = useState('');

//     members.forEach((member) => {
//         memberMap.set(member.id, member.name);
//     });

//     const [error, setError] = useState<string | null>(null);
//     const [showError, setShowError] = useState(false);

//     useEffect(() => {
//         const connect = new HubConnectionBuilder()
//             .withUrl(`/chat?conversationID=${conversationID}`)
//             .withAutomaticReconnect()
//             .build();

//         setConnection(connect);

//         return () => {
//             connection?.stop();
//         };
//     }, []);

//     useEffect(() => {
//         if (!connection) {
//             return;
//         }

//         connection
//             .start()
//             .then(() => {
//                 connection.on(
//                     'ReceiveMessage',
//                     ({ accountID, message }: { accountID: string; message: string }) => {
//                         const name = memberMap.get(accountID) ?? 'Unknown';

//                         setMessages((prevMessages) => [
//                             ...prevMessages,
//                             <div key={Date.now() + accountID}>
//                                 {name} - {message}
//                             </div>,
//                         ]);
//                     }
//                 );

//                 connection.on('ReceiveError', (err: string) => {
//                     setError(err);
//                     setShowError(true);
//                 });

//                 connection.on('UserConnected', (id) => {
//                     const name = memberMap.get(id) ?? 'Unknown';

//                     setMessages((prevMessages) => [
//                         ...prevMessages,
//                         <div key={Date.now() + id} className="text-danger">
//                             {name} Left!
//                         </div>,
//                     ]);
//                 });

//                 connection.on('UserDisconnected', (id) => {
//                     const name = memberMap.get(id) ?? 'Unknown';

//                     setMessages((prevMessages) => [
//                         ...prevMessages,
//                         <div key={Date.now() + id} className="text-success">
//                             {name} Joined!
//                         </div>,
//                     ]);
//                 });
//             })
//             .catch((error: Error) => {
//                 setError(error?.message);
//                 setShowError(true);
//             });
//     }, [connection]);

//     async function sendMessage() {
//         if (input.length > 1000 || input.length === 0) {
//             setError('Message must be between 1,000 and 0 characters.');
//             setShowError(true);
//             return;
//         }

//         if (!connection) return;

//         await connection.send('SendMessage', input);
//         setInput('');
//     }

//     return (
//         <>
//             <ToastContainer position="top-end">
//                 <Toast
//                     onClose={() => setShowError(false)}
//                     show={showError}
//                     autohide={true}
//                     className="d-inline-block m-1"
//                     bg={'danger'}
//                 >
//                     <Toast.Header>
//                         <strong className="me-auto">An error occured!</strong>
//                     </Toast.Header>
//                     <Toast.Body>{error}</Toast.Body>
//                 </Toast>
//             </ToastContainer>

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

// function Conversations({
//     setConvoID,
// }: {
//     setConvoID: Dispatch<SetStateAction<string | null>>;
// }) {
//     const res = useQuery<AxiosResponse<ChatPartial>, AxiosError<APIError<object>>>({
//         queryKey: [''],
//         queryFn: () => Endpoints.Conversations.get(),
//     });

//     const conversations = res.data?.data;
//     const error = res.error;

//     const [toastError, setToastError] = useState<APIError<object> | null>(null);
//     const [showError, setShowError] = useState(false);

//     useEffect(() => {
//         if (error) {
//             if (axios.isAxiosError<APIError<object>>(error) && error.response?.data) {
//                 setToastError(error.response.data);
//                 setShowError(true);
//                 console.log(toastError);
//             }
//         }
//     }, [error]);

//     return (
//         <div className="text-center">
//             <ToastContainer position="top-end">
//                 <Toast
//                     onClose={() => setShowError(!showError)}
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

//             <ul className="list-group fs-5">
//                 {conversations
//                     ? conversations?.map((convo) => (
//                           <li
//                               className="list-group-item bg-dark-subtle text-primary border-secondary"
//                               key={convo.id}
//                               style={{ cursor: 'pointer' }}
//                               onClick={() => {
//                                   setConvoID(convo.id);
//                               }}
//                           >
//                               {convo.name ?? 'Unknown'}
//                               <div className="fs-6">
//                                   Created -{' '}
//                                   {new Date(convo.createdAt).toLocaleDateString('en-US', {
//                                       weekday: 'long',
//                                       year: 'numeric',
//                                       month: 'long',
//                                       day: 'numeric',
//                                   })}
//                               </div>
//                           </li>
//                       ))
//                     : 'No Conversations'}
//             </ul>
//         </div>
//     );
// }

// function CreateConvo() {
//     const schema = yup.object().shape({
//         conversationName: yup
//             .string()
//             .required('conversationName is a required field.')
//             .min(1, 'Must be at least 1 characters.')
//             .max(25, 'Cannot be more than 25 characters.'),
//     });

//     async function createConvo(values: { conversationName: string }) {
//         try {
//             await Endpoints.Conversations.create(values.conversationName);
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
//                 Create Convo
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
//                                 onSubmit={createConvo}
//                                 initialValues={{ conversationName: '' }}
//                             >
//                                 {({ handleSubmit, handleChange, values, errors }) => (
//                                     <Form noValidate onSubmit={handleSubmit}>
//                                         <Row className="mb-3">
//                                             <Form.Group as={Col} controlId="conversationName">
//                                                 <Form.Label>Name</Form.Label>
//                                                 <Form.Control
//                                                     type="text"
//                                                     name="conversationName"
//                                                     value={values.conversationName}
//                                                     onChange={handleChange}
//                                                     isInvalid={!!errors.conversationName}
//                                                 />
//                                                 <Form.Control.Feedback type="invalid">
//                                                     {errors.conversationName}
//                                                 </Form.Control.Feedback>
//                                             </Form.Group>
//                                         </Row>
//                                         <Button type="submit">Create</Button>
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

// function Member({ id, name }: { id: string; name: string }) {
//     const schema = yup.object().shape({
//         message: yup
//             .string()
//             .min(1, 'Must be at least 1 characters.')
//             .max(250, 'Cannot be more than 250 characters.'),
//     });

//     async function sendRequest(values: { message: string }) {
//         try {
//             await Endpoints.Friends.Requests.send({
//                 recipientID: id,
//                 message: values.message.length !== 0 ? values.message : undefined,
//             });
//             setShowModal(false);
//             setShowSuccess(true);
//         } catch (err: unknown) {
//             if (axios.isAxiosError<APIError<object>>(err) && err.response?.data) {
//                 setToastError(err.response.data);
//                 setShowError(true);
//                 console.log(toastError);
//             }
//         }
//     }

//     const [toastError, setToastError] = useState<APIError<object> | null>(null);
//     const [showSuccess, setShowSuccess] = useState(false);
//     const [showError, setShowError] = useState(false);
//     const [showModal, setShowModal] = useState(false);

//     return (
//         <>
//             <li
//                 className="list-group-item bg-dark-subtle text-primary border-secondary"
//                 key={id}
//                 style={{ cursor: 'pointer' }}
//                 onClick={() => setShowModal(true)}
//             >
//                 {name ?? 'Unknown'}
//             </li>

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
//                 <Toast
//                     onClose={() => setShowSuccess(false)}
//                     show={showSuccess}
//                     autohide={true}
//                     className="d-inline-block m-1"
//                     bg={'success'}
//                 >
//                     <Toast.Header>
//                         <strong className="me-auto">Friend Request has been sent!</strong>
//                     </Toast.Header>
//                     <Toast.Body>
//                         <a href="/requests">view it here</a>
//                     </Toast.Body>
//                 </Toast>
//             </ToastContainer>

//             <Modal show={showModal}>
//                 <Modal.Dialog>
//                     <Modal.Header closeButton onClick={() => setShowModal(false)}>
//                         Send Friend Request?
//                     </Modal.Header>
//                     <Modal.Body>
//                         <div className="w-100">
//                             <Formik
//                                 validationSchema={schema}
//                                 validateOnChange
//                                 onSubmit={sendRequest}
//                                 initialValues={{ message: '' }}
//                             >
//                                 {({ handleSubmit, handleChange, values, errors }) => (
//                                     <Form noValidate onSubmit={handleSubmit}>
//                                         <Row className="mb-3">
//                                             <Form.Group as={Col} controlId="conversationID">
//                                                 <Form.Label>Message</Form.Label>
//                                                 <Form.Control
//                                                     as="textarea"
//                                                     type="textarea"
//                                                     rows={4}
//                                                     name="message"
//                                                     value={values.message}
//                                                     onChange={handleChange}
//                                                     isInvalid={!!errors.message}
//                                                 />
//                                                 <Form.Control.Feedback type="invalid">
//                                                     {errors.message}
//                                                 </Form.Control.Feedback>
//                                             </Form.Group>
//                                         </Row>
//                                         <Button type="submit">Send</Button>
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
