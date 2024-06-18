import { ToastContainer, Toast, Button, Modal, Form, Row, Col, InputGroup } from 'react-bootstrap';
import { redirectIfNotLoggedIn } from '../helpers';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import Navbar from '../components/navbar';
import Endpoints from '../endpoints';
import { useState } from 'react';
import { Formik } from 'formik';
import * as yup from 'yup';
import axios from 'axios';

type UpdateError = {
    newData: {
        email?: string;
        displayName?: string;
        password?: string;
    };
    ''?: string;
    confirmPassword?: string;
};

export default function Account() {
    redirectIfNotLoggedIn();

    const navigate = useNavigate();

    const { data } = useQuery<AccountData>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Account.get(),
    });

    const [showUpdateModal, setShowUpdateModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);

    const [showError, setShowError] = useState(false);
    const [toastError, setToastError] = useState<APIErrorRes<UpdateError | Record<string, never>> | null>(
        null
    );

    const newDataSchema = yup.object().shape({
        displayName: yup
            .string()
            .min(1, 'Must be at least 1 character.')
            .max(25, 'Cannot be more than 25 characters.'),
        email: yup.string().email('Must be a valid email.'),
        password: yup
            .string()
            .min(10, 'Must be at least 10 characters.')
            .max(70, 'Cannot be more than 70 characters.'),
    });

    const updateAccountSchema = yup.object().shape({
        newData: newDataSchema,
        currentPassword: yup.string().required('Current Password is a required field.'),
    });

    async function validateAccount(values: {
        displayName: string;
        email: string;
        password: string;
        currentPassword: string;
    }) {
        const errors: {
            displayName?: string;
            email?: string;
            password?: string;
            currentPassword?: string;
        } = {};

        if (values.email && values.email === data?.email) {
            errors.email = 'Email cannot be the same.';
        } else if (values.password && values.currentPassword && values.password === values.currentPassword) {
            errors.password = 'Passwords cannot be the same.';
        }

        let hasUpperCase = false;
        let hasLowerCase = false;
        let hasDigit = false;

        if (values.password.length > 0) {
            values.password.split('').forEach((char) => {
                if (char.toLocaleLowerCase() === char) {
                    hasLowerCase = true;
                }

                if (char.toLocaleUpperCase() === char) {
                    hasUpperCase = true;
                }

                if (!isNaN(Number(char))) {
                    hasDigit = true;
                }
            });

            if (!hasLowerCase) {
                errors.password = 'Missing a lower case letter.';
            } else if (!hasUpperCase) {
                errors.password = 'Missing an upper case letter.';
            } else if (!hasDigit) {
                errors.password = 'Missing a digit.';
            }
        }

        if (values.displayName.length === 0 && values.email.length === 0 && values.password.length === 0) {
            errors.displayName = 'Must update something.';
            errors.email = 'Must update something.';
            errors.password = 'Must update something.';
        }

        return errors;
    }

    async function updateAccount(values: {
        displayName: string;
        email: string;
        password: string;
        currentPassword: string;
    }): Promise<void> {
        const update: {
            displayName: string | null;
            email: string | null;
            password: string | null;
        } = {
            displayName: null,
            email: null,
            password: null,
        };

        if (values.displayName) {
            update.displayName = values.displayName;
        }

        if (values.email) {
            update.email = values.email;
        }

        if (values.password) {
            update.password = values.password;
        }

        try {
            await Endpoints.Account.update({
                newData: update,
                currentPassword: values.currentPassword,
            });

            data!.displayName = update.displayName ?? data!.displayName;
            data!.email = update.email ?? data!.email;

            setShowUpdateModal(false);
        } catch (err) {
            if (axios.isAxiosError<APIErrorRes<UpdateError>>(err) && err.response?.data) {
                setToastError(err.response.data);
                setShowError(true);
            }
        }
    }

    return (
        <>
            <Navbar />
            <div className="container w-50 align-items-center justify-content-center text-center">
                <br />
                <div className="row">
                    <div className="text-center bg-white rounded shadow card-body p-3 bg-primary">
                        <h1 className="mb-2">{data?.displayName || 'Unkown'}</h1>
                        <h5 className="mb-2">{data?.email}</h5>
                        <h5 className="mb-2">
                            Created:{' '}
                            {data?.createdAt
                                ? new Date(data.createdAt).toLocaleDateString('en-US', {
                                      weekday: 'long',
                                      year: 'numeric',
                                      month: 'long',
                                      day: 'numeric',
                                  })
                                : 'Unkown'}
                        </h5>
                        <br />
                        <h5 className="mb-2">Total Friends: {data?.totalFriends ?? 'Unknown'}</h5>
                        <h5 className="mb-2">Total Convos: {data?.totalConversations ?? 'Unknown'}</h5>
                        <br />
                        <Row className="justify-content-center">
                            <a
                                onClick={() => setShowUpdateModal(true)}
                                className="btn btn-warning btn-lg w-25 m-2"
                                role="button"
                            >
                                Update
                            </a>
                            <a
                                onClick={() => setShowDeleteModal(true)}
                                className="btn btn-danger btn-lg w-25 m-2"
                                role="button"
                            >
                                Delete
                            </a>
                            <a
                                className="btn btn-info btn-lg w-25 m-2"
                                role="button"
                                onClick={() => {
                                    navigator.clipboard.writeText(data!.id);
                                }}
                                style={{ width: '10%' }}
                            >
                                Copy ID
                            </a>
                        </Row>
                    </div>
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
                                    return <div key={err.toString()}>{err.toString()}</div>;
                                })}
                        </Toast.Body>
                    </Toast>
                </ToastContainer>

                <Modal show={showUpdateModal}>
                    <Modal.Dialog>
                        <Modal.Header
                            closeButton
                            onClick={() => setShowUpdateModal(!showUpdateModal)}
                        ></Modal.Header>

                        <Modal.Body>
                            <div className="w-100">
                                <Formik
                                    validationSchema={updateAccountSchema}
                                    validate={validateAccount}
                                    validateOnChange
                                    onSubmit={updateAccount}
                                    initialValues={{
                                        displayName: '',
                                        email: '',
                                        password: '',
                                        currentPassword: '',
                                    }}
                                >
                                    {({ handleSubmit, handleChange, values, errors }) => (
                                        <Form noValidate onSubmit={handleSubmit}>
                                            <Row className="mb-3">
                                                <Form.Group as={Col} controlId="currentPasswordID">
                                                    <Form.Label>Current Password</Form.Label>
                                                    <Form.Control
                                                        type="password"
                                                        name="currentPassword"
                                                        value={values.currentPassword}
                                                        onChange={handleChange}
                                                        isInvalid={!!errors.currentPassword}
                                                    />
                                                    <Form.Control.Feedback type="invalid">
                                                        {errors.currentPassword}
                                                    </Form.Control.Feedback>
                                                </Form.Group>
                                            </Row>
                                            <Row className="mb-3">
                                                <Form.Group as={Col} controlId="DisplayNameID">
                                                    <Form.Label>New DisplayName</Form.Label>
                                                    <InputGroup hasValidation>
                                                        <Form.Control
                                                            type="text"
                                                            placeholder="DisplayName"
                                                            aria-describedby="inputGroupPrepend"
                                                            name="displayName"
                                                            value={values.displayName}
                                                            onChange={handleChange}
                                                            isInvalid={!!errors.displayName}
                                                        />
                                                        <Form.Control.Feedback type="invalid">
                                                            {errors.displayName}
                                                        </Form.Control.Feedback>
                                                    </InputGroup>
                                                </Form.Group>
                                            </Row>
                                            <Row className="mb-3">
                                                <Form.Group as={Col} controlId="EmailID">
                                                    <Form.Label>Email</Form.Label>
                                                    <InputGroup hasValidation>
                                                        <Form.Control
                                                            type="email"
                                                            aria-describedby="inputGroupPrepend"
                                                            placeholder="example@email.com"
                                                            name="email"
                                                            value={values.email}
                                                            onChange={handleChange}
                                                            isInvalid={!!errors.email}
                                                        />
                                                        <Form.Control.Feedback type="invalid">
                                                            {errors.email}
                                                        </Form.Control.Feedback>
                                                    </InputGroup>
                                                </Form.Group>
                                            </Row>
                                            <Row className="mb-3">
                                                <Form.Group as={Col} controlId="PasswordID">
                                                    <Form.Label>New Password</Form.Label>
                                                    <InputGroup hasValidation>
                                                        <Form.Control
                                                            type="password"
                                                            aria-describedby="inputGroupPrepend"
                                                            name="password"
                                                            value={values.password}
                                                            onChange={handleChange}
                                                            isInvalid={!!errors.password}
                                                        />
                                                        <Form.Control.Feedback type="invalid">
                                                            {errors.password}
                                                        </Form.Control.Feedback>
                                                    </InputGroup>
                                                </Form.Group>
                                            </Row>
                                            <Button type="submit">Update</Button>
                                        </Form>
                                    )}
                                </Formik>
                            </div>
                        </Modal.Body>
                    </Modal.Dialog>
                </Modal>

                <Modal show={showDeleteModal}>
                    <Modal.Dialog>
                        <Modal.Header closeButton onClick={() => setShowDeleteModal(false)}></Modal.Header>

                        <Modal.Body>Are you sure you want to delete your account?</Modal.Body>
                        <Modal.Footer>
                            <Button
                                variant="danger"
                                onClick={async () => {
                                    try {
                                        await Endpoints.Account.delete();

                                        setShowUpdateModal(false);
                                        navigate('/auth');
                                    } catch (err) {
                                        if (
                                            axios.isAxiosError<APIErrorRes<Record<string, never>>>(err) &&
                                            err.response?.data
                                        ) {
                                            setToastError(err.response.data);
                                            setShowError(true);
                                        }
                                    }
                                }}
                            >
                                Delete
                            </Button>
                        </Modal.Footer>
                    </Modal.Dialog>
                </Modal>
            </div>
        </>
    );
}
