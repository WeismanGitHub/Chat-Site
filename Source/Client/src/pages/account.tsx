import { ToastContainer, Toast, Button, Modal, Form, Row, Col, InputGroup, Card } from 'react-bootstrap';
import { redirectIfNotLoggedIn } from '../helpers';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import Navbar from '../navbar';
import { Formik } from 'formik';
import * as yup from 'yup';
import axios from 'axios';

type UpdateError = {
    newData: {
        name?: string;
        password?: string;
    };
    ''?: string;
    confirmPassword?: string;
};

type CustomError = APIError<UpdateError | Record<string, never>> | null;

const newDataSchema = yup.object().shape({
    name: yup.string().min(1, 'Must be at least 1 character.').max(25, 'Cannot be more than 25 characters.'),
    password: yup
        .string()
        .min(10, 'Must be at least 10 characters.')
        .max(70, 'Cannot be more than 70 characters.'),
});

const updateAccountSchema = yup.object().shape({
    newData: newDataSchema,
    currentPassword: yup.string().required('Current Password is a required field.'),
});

export default function Account() {
    redirectIfNotLoggedIn();

    const navigate = useNavigate();
    const [account, setAccount] = useState<Account | null>(null);

    const [showUpdateModal, setShowUpdateModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);

    const [showError, setShowError] = useState(false);
    const [toastError, setToastError] = useState<CustomError>(null);

    useEffect(() => {
        axios
            .get<Account>('/API/Account/v1')
            .then((res) => setAccount(res.data))
            .catch((err: unknown) => {
                if (axios.isAxiosError<CustomError>(err) && err.response?.data) {
                    if (err.response?.data?.statusCode == 401) {
                        localStorage.removeItem('authenticated');
                        return navigate('/auth');
                    }

                    setToastError(err.response.data);
                } else {
                    setToastError({
                        message: 'Unknown Error',
                        errors: {},
                        statusCode: 500,
                    });
                }
            });
    }, []);

    return (
        <>
            <Navbar />
            <div className="d-flex justify-content-center align-items-center full-height-minus-navbar overflow-x-hidden">
                <Card style={{ maxWidth: '600px', width: '80%' }} className="mx-auto shadow">
                    <Card.Header className="bg-primary text-white">
                        <h2>{account?.name ?? 'Unknown'}</h2>
                    </Card.Header>
                    <Card.Body>
                        <Row>
                            <Card.Text className='mb-2'>
                                <strong>Created:</strong>{' '}
                                {account &&
                                    new Date(account?.createdAt).toLocaleDateString('en-US', {
                                        weekday: 'long',
                                        year: 'numeric',
                                        month: 'long',
                                        day: 'numeric',
                                    })}
                                <br />
                                <strong>Chats:</strong> {account?.chatRooms ?? 'Unknown'}
                            </Card.Text>
                        </Row>
                        <Card.Footer>
                            <Row className="justify-content-end">
                                <Button
                                    style={{ width: '90px', marginRight: '3px' }}
                                    variant="warning"
                                    onClick={() => setShowUpdateModal(true)}
                                >
                                    Update
                                </Button>
                                <Button
                                    style={{ width: '90px' }}
                                    variant="danger"
                                    onClick={() => setShowDeleteModal(true)}
                                >
                                    Delete
                                </Button>
                            </Row>
                        </Card.Footer>
                    </Card.Body>
                </Card>
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
                        {account && (
                            <Formik
                                validationSchema={updateAccountSchema}
                                validate={(values) => {
                                    const errors: {
                                        name?: string;
                                        password?: string;
                                        currentPassword?: string;
                                    } = {};

                                    if (
                                        values.password &&
                                        values.currentPassword &&
                                        values.password === values.currentPassword
                                    ) {
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

                                    if (values.name.length === 0 && values.password.length === 0) {
                                        errors.name = 'Must update something.';
                                        errors.password = 'Must update something.';
                                    }

                                    return errors;
                                }}
                                validateOnChange
                                onSubmit={async (values) => {
                                    const update: {
                                        name: string | null;
                                        password: string | null;
                                    } = {
                                        name: null,
                                        password: null,
                                    };

                                    if (values.name) {
                                        update.name = values.name;
                                    }

                                    if (values.password) {
                                        update.password = values.password;
                                    }

                                    try {
                                        await axios.patch('/API/Account/v1', {
                                            newData: update,
                                            currentPassword: values.currentPassword,
                                        });

                                        setAccount({
                                            id: account.id,
                                            chatRooms: account.chatRooms,
                                            createdAt: account.createdAt,
                                            name: update.name ?? account.name,
                                        });

                                        setShowUpdateModal(false);
                                    } catch (err) {
                                        if (
                                            axios.isAxiosError<APIError<UpdateError>>(err) &&
                                            err.response?.data
                                        ) {
                                            setToastError(err.response.data);
                                            setShowError(true);
                                        }
                                    }
                                }}
                                initialValues={{
                                    name: '',
                                    password: '',
                                    currentPassword: '',
                                }}
                            >
                                {({ handleSubmit, handleChange, values, errors }) => (
                                    <Form noValidate onSubmit={handleSubmit}>
                                        <Row className="mb-3">
                                            <Form.Group as={Col} controlId="nameID">
                                                <Form.Label>New Name</Form.Label>
                                                <InputGroup hasValidation>
                                                    <Form.Control
                                                        type="text"
                                                        placeholder="name"
                                                        aria-describedby="inputGroupPrepend"
                                                        name="name"
                                                        value={values.name}
                                                        onChange={handleChange}
                                                        isInvalid={!!errors.name}
                                                    />
                                                    <Form.Control.Feedback type="invalid">
                                                        {errors.name}
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
                                        <Button type="submit">Update</Button>
                                    </Form>
                                )}
                            </Formik>
                        )}
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
                                    await axios.delete('/api/account/v1');

                                    setShowUpdateModal(false);
                                    navigate('/auth');
                                } catch (err) {
                                    if (
                                        axios.isAxiosError<APIError<Record<string, never>>>(err) &&
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
        </>
    );
}
