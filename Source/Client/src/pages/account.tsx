import {
    ToastContainer,
    Toast,
    Button,
    Modal,
    Form,
    Row,
    Col,
    InputGroup,
    Card,
    FormGroup,
    FormControl,
    FormLabel,
} from 'react-bootstrap';
import { redirectIfNotLoggedIn } from '../helpers';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import Navbar from '../navbar';
import { Formik } from 'formik';
import * as yup from 'yup';
import axios from 'axios';
import { passwordSchema } from '../schemas';

type UpdateError = {
    newData: {
        name?: string;
        password?: string;
    };
    ''?: string;
    confirmPassword?: string;
};

type CustomError = APIError<UpdateError | Record<string, string>> | null;

const newDataSchema = yup.object().shape({
    name: yup.string().min(1, 'Must be at least 1 character.').max(25, 'Cannot be more than 25 characters.'),
    newPassword: yup
        .string()
        .min(10, 'Must be at least 10 characters.')
        .max(70, 'Cannot be more than 70 characters.'),
});

const updateAccountSchema = yup.object().shape({
    newData: newDataSchema,
    password: yup.string().required('Current Password is a required field.'),
});

export default function Account() {
    redirectIfNotLoggedIn();

    const navigate = useNavigate();
    const [account, setAccount] = useState<Account | null>(null);

    const [showUpdateModal, setShowUpdateModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);

    const [error, setError] = useState<CustomError>(null);

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

                    setError(err.response.data);
                } else {
                    setError({
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
                            <Card.Text className="mb-2">
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

            {account && (
                <Formik
                    validationSchema={yup.object().shape({
                        password: passwordSchema.required('Input your current password.'),
                    })}
                    validateOnMount
                    validateOnChange
                    initialValues={{
                        password: '',
                    }}
                    onSubmit={async (values) => {
                        try {
                            await axios.delete('/Api/Account/v1', {
                                data: {
                                    password: values.password,
                                },
                            });

                            localStorage.removeItem('authenticated');
                            navigate('/auth');
                        } catch (err) {
                            if (axios.isAxiosError<CustomError>(err) && err.response?.data) {
                                if (err.response?.data?.statusCode == 401) {
                                    localStorage.removeItem('authenticated');
                                    return navigate('/auth');
                                }

                                setError(err.response.data);
                            } else {
                                setError({
                                    message: 'Unknown Error',
                                    errors: {},
                                    statusCode: 500,
                                });
                            }
                        }
                    }}
                >
                    {({ handleSubmit, handleChange, values, errors }) => (
                        <Modal
                            show={showDeleteModal}
                            centered
                            keyboard={true}
                            onHide={() => setShowDeleteModal(false)}
                            animation={false}
                        >
                            <Form noValidate onSubmit={handleSubmit}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Delete your account?</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="PasswordId">
                                            <FormLabel>Password</FormLabel>
                                            <InputGroup hasValidation>
                                                <FormControl
                                                    autoComplete="on"
                                                    type="password"
                                                    aria-describedby="inputGroupPrepend"
                                                    name="password"
                                                    value={values.password}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.password}
                                                />
                                                <FormControl.Feedback type="invalid">
                                                    {errors.password}
                                                </FormControl.Feedback>
                                            </InputGroup>
                                        </FormGroup>
                                    </Row>
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button type="submit" variant="danger">
                                        Delete
                                    </Button>
                                    <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
                                        Close
                                    </Button>
                                </Modal.Footer>
                            </Form>
                        </Modal>
                    )}
                </Formik>
            )}

            {account && (
                <Formik
                    validationSchema={updateAccountSchema}
                    validate={(values) => {
                        const errors: {
                            name?: string;
                            password?: string;
                            newPassword?: string;
                        } = {};

                        if (
                            values.password &&
                            values.newPassword &&
                            values.password === values.newPassword
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
                                errors.newPassword = 'Missing a lower case letter.';
                            } else if (!hasUpperCase) {
                                errors.newPassword = 'Missing an upper case letter.';
                            } else if (!hasDigit) {
                                errors.newPassword = 'Missing a digit.';
                            }
                        }

                        if (values.name.length === 0 && values.newPassword.length === 0) {
                            errors.name = 'Must update something.';
                            errors.newPassword = 'Must update something.';
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

                        if (values.newPassword) {
                            update.password = values.newPassword;
                        }

                        try {
                            await axios.patch('/API/Account/v1', {
                                newData: update,
                                currentPassword: values.password,
                            });

                            setAccount({
                                id: account.id,
                                chatRooms: account.chatRooms,
                                createdAt: account.createdAt,
                                name: update.name ?? account.name,
                            });

                            setShowUpdateModal(false);
                        } catch (err) {
                            if (axios.isAxiosError<APIError<UpdateError>>(err) && err.response?.data) {
                                setError(err.response.data);
                            }
                        }
                    }}
                    initialValues={{
                        name: '',
                        password: '',
                        newPassword: '',
                    }}
                >
                    {({ handleSubmit, handleChange, values, errors }) => (
                        <Modal
                            show={showUpdateModal}
                            centered
                            keyboard={true}
                            onHide={() => setShowUpdateModal(false)}
                            animation={false}
                        >
                            <Form noValidate onSubmit={handleSubmit}>
                                <Modal.Header closeButton>
                                    <Modal.Title>Update your account?</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="NewNameID">
                                            <FormLabel>New Name</FormLabel>
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
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="NewPasswordId">
                                            <FormLabel>New Password</FormLabel>
                                            <InputGroup hasValidation>
                                                <FormControl
                                                    type="password"
                                                    autoComplete="off"
                                                    aria-describedby="inputGroupPrepend"
                                                    name="newPassword"
                                                    value={values.newPassword}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.newPassword}
                                                />
                                                <FormControl.Feedback type="invalid">
                                                    {errors.newPassword}
                                                </FormControl.Feedback>
                                            </InputGroup>
                                        </FormGroup>
                                    </Row>
                                    <Row className="mb-3">
                                        <FormGroup as={Col} controlId="CurrentPasswordId">
                                            <FormLabel>Current Password</FormLabel>
                                            <InputGroup hasValidation>
                                                <FormControl
                                                    autoComplete="on"
                                                    type="password"
                                                    aria-describedby="inputGroupPrepend"
                                                    name="password"
                                                    value={values.password}
                                                    onChange={handleChange}
                                                    isInvalid={!!errors.password}
                                                />
                                                <FormControl.Feedback type="invalid">
                                                    {errors.password}
                                                </FormControl.Feedback>
                                            </InputGroup>
                                        </FormGroup>
                                    </Row>
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button type="submit" variant="warning">
                                        Update
                                    </Button>
                                    <Button variant="secondary" onSubmit={() => setShowUpdateModal(false)}>
                                        Close
                                    </Button>
                                </Modal.Footer>
                            </Form>
                        </Modal>
                    )}
                </Formik>
            )}

            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setError(null)}
                    show={error !== null}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">{error?.message || 'Unable to read error name.'}</strong>
                    </Toast.Header>
                    <Toast.Body>
                        {error?.errors &&
                            Object.values(error?.errors).map((err) => {
                                return <div key={err.toString()}>{err.toString()}</div>;
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </>
    );
}
