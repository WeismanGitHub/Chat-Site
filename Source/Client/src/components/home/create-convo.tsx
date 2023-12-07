import { ToastContainer, Toast, Modal, Button, Row, Form, Col } from 'react-bootstrap';
import { Dispatch, SetStateAction, useState } from 'react';
import Endpoints from '../../endpoints';
import { Formik } from 'formik';
import { HTTPError } from 'ky';
import * as yup from 'yup';

export default function CreateConvo({
    setConversations,
    conversations,
}: {
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
    conversations: ConversationsData;
}) {
    const schema = yup.object().shape({
        conversationName: yup
            .string()
            .required('conversationName is a required field.')
            .min(1, 'Must be at least 1 characters.')
            .max(50, 'Cannot be more than 25 characters.'),
    });

    async function createConvo(values: { conversationName: string }) {
        try {
            const res = await Endpoints.Conversations.create(values.conversationName);

            setConversations([
                ...conversations,
                {
                    id: res.conversationID,
                    name: values.conversationName,
                    createdAt: String(new Date()),
                },
            ]);
            setShowModal(false);
        } catch (err: unknown) {
            if (err instanceof HTTPError) {
                setToastError(await err.response.json());
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    const [toastError, setToastError] = useState<APIErrorRes<object> | null>(null);
    const [showError, setShowError] = useState(false);
    const [showModal, setShowModal] = useState(false);

    return (
        <>
            <div className="btn btn-outline-primary" onClick={() => setShowModal(true)}>
                Create Convo
            </div>

            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowError(false)}
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
                    <Modal.Header closeButton onClick={() => setShowModal(false)}></Modal.Header>
                    <Modal.Body>
                        <div className="w-100">
                            <Formik
                                validationSchema={schema}
                                validateOnChange
                                onSubmit={createConvo}
                                initialValues={{ conversationName: '' }}
                            >
                                {({ handleSubmit, handleChange, values, errors }) => (
                                    <Form noValidate onSubmit={handleSubmit}>
                                        <Row className="mb-3">
                                            <Form.Group as={Col} controlId="conversationName">
                                                <Form.Label>Name</Form.Label>
                                                <Form.Control
                                                    type="text"
                                                    name="conversationName"
                                                    value={values.conversationName}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.conversationName}
                                                />
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.conversationName}
                                                </Form.Control.Feedback>
                                            </Form.Group>
                                        </Row>
                                        <Button type="submit">Create</Button>
                                    </Form>
                                )}
                            </Formik>
                        </div>
                    </Modal.Body>
                </Modal.Dialog>
            </Modal>
        </>
    );
}
