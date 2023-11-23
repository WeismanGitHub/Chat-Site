import {
    ToastContainer,
    Toast,
    Button,
    Modal,
    Form,
    Row,
    Col,
    InputGroup,
} from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Navbar from '../components/navbar';
import Endpoints from '../endpoints';
import ky, { HTTPError } from 'ky';
import { useState } from 'react';
import { Formik } from 'formik';
import * as yup from 'yup';

type accountData = {
    id: string;
    displayName: string;
    email: string;
    totalConversations: number;
    totalFriends: number;
    createdAt: string;
};

export default function Account() {
    // eslint-disable-next-line prefer-const
    let { error, data } = useQuery<accountData, HTTPError>({
        queryKey: ['data'],
        queryFn: (): Promise<accountData> =>
            ky.get(Endpoints.Account.Route()).json(),
    });

    const [showUpdateModal, setShowUpdateModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);

    const toggleError = () => setShowError(!showError);
    const [showError, setShowError] = useState(false);

    if (error) {
        setShowError(true);
    }

    const schema = yup.object().shape({
        displayName: yup.string().required().min(1).max(25),
        email: yup.string().required().email('Must be a valid email.'),
        password: yup.string().required().min(10).max(70),
        confirmPassword: yup.string().required(),
    });

    return (
        <>
            <Navbar />
            <div className="container w-50 align-items-center justify-content-center text-center">
                <br />
                <div className="row">
                    <div className="text-center bg-white rounded shadow card-body p-3 bg-primary">
                        <h1 className="mb-2">
                            {data?.displayName || 'Unkown'}
                        </h1>
                        <h5 className="mb-2">{data?.email}</h5>
                        <h5 className="mb-2">
                            Created:{' '}
                            {data?.createdAt
                                ? new Date(data.createdAt).toLocaleDateString(
                                      'en-US',
                                      {
                                          weekday: 'long',
                                          year: 'numeric',
                                          month: 'long',
                                          day: 'numeric',
                                      }
                                  )
                                : 'Unkown'}
                        </h5>
                        <br />
                        <h5 className="mb-2">
                            Total Friends: {data?.totalFriends ?? 'Unknown'}
                        </h5>
                        <h5 className="mb-2">
                            Total Convos:{' '}
                            {data?.totalConversations ?? 'Unknown'}
                        </h5>
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
                        </Row>
                    </div>
                </div>

                <ToastContainer position="top-end">
                    <Toast
                        onClose={toggleError}
                        show={showError}
                        autohide={true}
                        className="d-inline-block m-1"
                        bg={'danger'}
                    >
                        <Toast.Header>
                            <strong className="me-auto">
                                {error?.name || 'Unable to read error name.'}
                            </strong>
                        </Toast.Header>
                        <Toast.Body>
                            {error?.message || 'Unable to read error message.'}
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
                            <Formik
                                validationSchema={schema}
                                validate={(values) => {
                                    const errors: {
                                        password?: string;
                                        confirmPassword?: string;
                                    } = {};

                                    if (
                                        values.password !==
                                        values.confirmPassword
                                    ) {
                                        errors.confirmPassword =
                                            'Passwords do not match.';
                                    }

                                    let hasUpperCase = false;
                                    let hasLowerCase = false;
                                    let hasDigit = false;

                                    values.password
                                        .split('')
                                        .forEach((char) => {
                                            if (
                                                char.toLocaleLowerCase() ===
                                                char
                                            ) {
                                                hasLowerCase = true;
                                            }
                                            if (
                                                char.toLocaleUpperCase() ===
                                                char
                                            ) {
                                                hasUpperCase = true;
                                            }
                                            if (!isNaN(Number(char))) {
                                                hasDigit = true;
                                            }
                                        });

                                    if (!hasLowerCase) {
                                        errors.password =
                                            'Missing a lower case letter.';
                                    } else if (!hasUpperCase) {
                                        errors.password =
                                            'Missing an upper case letter.';
                                    } else if (!hasDigit) {
                                        errors.password = 'Missing a digit.';
                                    }

                                    return errors;
                                }}
                                validateOnChange
                                onSubmit={async (values) => {
                                    await ky
                                        .patch(Endpoints.Account.Route(), {
                                            json: {
                                                DisplayName: values.displayName,
                                                Email: values.email,
                                                Password: values.password,
                                            },
                                        })
                                        .catch((err: HTTPError) => {
                                            setShowError(true);
                                            error = err;
                                        });
                                }}
                                initialValues={{
                                    displayName: '',
                                    email: '',
                                    password: '',
                                    confirmPassword: '',
                                }}
                            >
                                {({
                                    handleSubmit,
                                    handleChange,
                                    values,
                                    errors,
                                }) => (
                                    <Form noValidate onSubmit={handleSubmit}>
                                        <Row className="mb-3">
                                            <Form.Group
                                                as={Col}
                                                controlId="DisplayNameID"
                                            >
                                                <Form.Label>
                                                    DisplayName
                                                </Form.Label>
                                                <InputGroup hasValidation>
                                                    <Form.Control
                                                        type="text"
                                                        placeholder="DisplayName"
                                                        aria-describedby="inputGroupPrepend"
                                                        name="displayName"
                                                        value={
                                                            values.displayName
                                                        }
                                                        onChange={handleChange}
                                                        isInvalid={
                                                            !!errors.displayName
                                                        }
                                                    />
                                                    <Form.Control.Feedback type="invalid">
                                                        {errors.displayName}
                                                    </Form.Control.Feedback>
                                                </InputGroup>
                                            </Form.Group>
                                        </Row>
                                        <Row className="mb-3">
                                            <Form.Group
                                                as={Col}
                                                controlId="EmailID"
                                            >
                                                <Form.Label>Email</Form.Label>
                                                <InputGroup hasValidation>
                                                    <Form.Control
                                                        type="email"
                                                        placeholder="example@email.com"
                                                        name="email"
                                                        value={values.email}
                                                        onChange={handleChange}
                                                        isInvalid={
                                                            !!errors.email
                                                        }
                                                    />
                                                </InputGroup>
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.email}
                                                </Form.Control.Feedback>
                                            </Form.Group>
                                        </Row>
                                        <Row className="mb-3">
                                            <Form.Group
                                                as={Col}
                                                controlId="PasswordID"
                                            >
                                                <Form.Label>
                                                    Password
                                                </Form.Label>
                                                <InputGroup hasValidation>
                                                    <Form.Control
                                                        type="password"
                                                        aria-describedby="inputGroupPrepend"
                                                        name="password"
                                                        value={values.password}
                                                        onChange={handleChange}
                                                        isInvalid={
                                                            !!errors.password
                                                        }
                                                    />
                                                    <Form.Control.Feedback type="invalid">
                                                        {errors.password}
                                                    </Form.Control.Feedback>
                                                </InputGroup>
                                            </Form.Group>
                                        </Row>
                                        <Row className="mb-3">
                                            <Form.Group
                                                as={Col}
                                                controlId="ConfirmPasswordID"
                                            >
                                                <Form.Label>Confirm</Form.Label>
                                                <Form.Control
                                                    type="password"
                                                    name="confirmPassword"
                                                    value={
                                                        values.confirmPassword
                                                    }
                                                    onChange={handleChange}
                                                    isInvalid={
                                                        !!errors.confirmPassword
                                                    }
                                                />
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.confirmPassword}
                                                </Form.Control.Feedback>
                                            </Form.Group>
                                        </Row>
                                        <Button type="submit">Sign Up</Button>
                                    </Form>
                                )}
                            </Formik>
                        </Modal.Body>
                    </Modal.Dialog>
                </Modal>

                <Modal show={showDeleteModal}>
                    <Modal.Dialog>
                        <Modal.Header
                            closeButton
                            onClick={() => setShowDeleteModal(false)}
                        ></Modal.Header>

                        <Modal.Body>
                            Are you sure you want to delete your account?
                        </Modal.Body>
                        <Modal.Footer>
                            <Button
                                variant="danger"
                                onClick={() => {
                                    console.log('delete');
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
