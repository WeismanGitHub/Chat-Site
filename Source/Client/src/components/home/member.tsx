import { ToastContainer, Toast, Modal, Button, Row, Form, Col } from 'react-bootstrap';
import Endpoints from '../../endpoints';
import { useState } from 'react';
import { Formik } from 'formik';
import { HTTPError } from 'ky';
import * as yup from 'yup';

export default function Member({ id, name }: { id: string, name: string }) {
    const schema = yup.object().shape({
        message: yup
            .string()
            .min(1, 'Must be at least 1 characters.')
            .max(250, 'Cannot be more than 250 characters.'),
    });

    async function sendRequest(values: {message: string}) {
        try {
            await Endpoints.Friends.Requests.send({
                recipientID: id,
                message: values.message.length !== 0 ? values.message : undefined,
            });
            setShowModal(false);
            setShowSuccess(true);
        } catch (err: unknown) {
            if (err instanceof HTTPError) {
                setToastError(await err.response.json());
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    const [toastError, setToastError] = useState<APIErrorRes<object> | null>(null);
    const [showSuccess, setShowSuccess] = useState(false);
    const [showError, setShowError] = useState(false);
    const [showModal, setShowModal] = useState(false);

    return (
        <>
            <li
                className="list-group-item bg-dark-subtle text-primary border-secondary"
                key={id}
                style={{ cursor: 'pointer' }}
                onClick={() => setShowModal(true)}
            >
                {name ?? 'Unknown'}
            </li>

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
                <Toast
                    onClose={() => setShowSuccess(false)}
                    show={showSuccess}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'success'}
                >
                    <Toast.Header>
                        <strong className="me-auto">Friend Request has been sent!</strong>
                    </Toast.Header>
                    <Toast.Body>
                        <a href="/requests">view it here</a>
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            <Modal show={showModal}>
                <Modal.Dialog>
                    <Modal.Header closeButton onClick={() => setShowModal(false)}>Send Friend Request?</Modal.Header>
                    <Modal.Body>
                        <div className="w-100">
                            <Formik
                                validationSchema={schema}
                                validateOnChange
                                onSubmit={sendRequest}
                                initialValues={{ message: '' }}
                            >
                                {({ handleSubmit, handleChange, values, errors }) => (
                                    <Form noValidate onSubmit={handleSubmit}>
                                        <Row className="mb-3">
                                            <Form.Group as={Col} controlId="conversationID">
                                                <Form.Label>Message</Form.Label>
                                                <Form.Control
                                                    as="textarea"
                                                    type="textarea"
                                                    rows={4}
                                                    name="message"
                                                    value={values.message}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.message}
                                                />
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.message}
                                                </Form.Control.Feedback>
                                            </Form.Group>
                                        </Row>
                                        <Button type="submit">Send</Button>
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
