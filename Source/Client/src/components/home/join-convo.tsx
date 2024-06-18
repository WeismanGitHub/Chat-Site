import { ToastContainer, Toast, Modal, Button, Row, Form, Col } from 'react-bootstrap';
import Endpoints from '../../endpoints';
import { useState } from 'react';
import { Formik } from 'formik';
import * as yup from 'yup';
import axios from 'axios';

export default function CreateConvo() {
    const schema = yup.object().shape({
        conversationID: yup
            .string()
            .required('Convo ID is a required field.')
            .min(1, 'Must be at least 1 characters.')
            .max(25, 'Cannot be more than 25 characters.'),
    });

    async function joinConvo(values: { conversationID: string }) {
        try {
            await Endpoints.Conversations.join(values.conversationID);
            window.location.reload();
            setShowModal(false);
        } catch (err: unknown) {
                if (axios.isAxiosError<APIErrorRes<object>>(err) && err.response?.data) {
                    setToastError(err.response.data)
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
                Join Convo
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
                                onSubmit={joinConvo}
                                initialValues={{ conversationID: '' }}
                            >
                                {({ handleSubmit, handleChange, values, errors }) => (
                                    <Form noValidate onSubmit={handleSubmit}>
                                        <Row className="mb-3">
                                            <Form.Group as={Col} controlId="conversationID">
                                                <Form.Label>Convo ID</Form.Label>
                                                <Form.Control
                                                    type="text"
                                                    name="conversationID"
                                                    value={values.conversationID}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.conversationID}
                                                />
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.conversationID}
                                                </Form.Control.Feedback>
                                            </Form.Group>
                                        </Row>
                                        <Button type="submit">Join</Button>
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
