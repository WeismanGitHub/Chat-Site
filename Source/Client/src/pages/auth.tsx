import { nameSchema, passwordSchema } from '../schemas';
import { useNavigate } from 'react-router-dom';
import Navbar from '../navbar';
import { useState } from 'react';
import { Formik } from 'formik';
import * as yup from 'yup';
import axios from 'axios';
import {
    Button,
    Card,
    Col,
    Form,
    FormControl,
    FormGroup,
    FormLabel,
    InputGroup,
    Row,
    Toast,
    ToastContainer,
} from 'react-bootstrap';

const signinSchema = yup.object().shape({
    name: nameSchema,
    password: passwordSchema,
});

const signupSchema = signinSchema.shape({
    confirmPassword: yup.string().required('Please confirm your password.'),
});

export default function Auth() {
    const [error, setError] = useState<APIError<object> | null>(null);
    const [showSignin, setShowSignin] = useState<boolean>(true);

    return (
        <>
            <Navbar />
            <div className="d-flex justify-content-center align-items-center full-height-minus-navbar overflow-x-hidden">
                <Card style={{ maxWidth: '500px', width: '80%' }} className="border-0 shadow">
                    <Card.Header className="bg-primary text-white">
                        <h2>{showSignin ? 'Sign In' : 'Sign Up'}</h2>
                    </Card.Header>
                    <Card.Body>
                        {showSignin ? (
                            <Signin
                                setError={setError}
                                showSignin={showSignin}
                                setShowSignin={setShowSignin}
                            />
                        ) : (
                            <Signup
                                setError={setError}
                                showSignin={showSignin}
                                setShowSignin={setShowSignin}
                            />
                        )}
                    </Card.Body>
                </Card>

                <ToastContainer position="top-end">
                    <Toast
                        onClose={() => setError(null)}
                        show={error !== null}
                        autohide={true}
                        className="d-inline-block m-1"
                        bg={'danger'}
                    >
                        <Toast.Header>
                            <strong className="me-auto">{error?.message ?? 'An error occurred!'}</strong>
                        </Toast.Header>
                        <Toast.Body className="text-white">
                            <strong>
                                {error?.errors &&
                                    Object.values(error?.errors).map((err) => {
                                        return <div key={err}>{err}</div>;
                                    })}
                            </strong>
                        </Toast.Body>
                    </Toast>
                </ToastContainer>
            </div>
        </>
    );
}

function Signup({
    setError,
    setShowSignin,
    showSignin,
}: {
    setError: setState<APIError<object> | null>;
    setShowSignin: setState<boolean>;
    showSignin: boolean;
}) {
    const navigate = useNavigate();

    return (
        <>
            <Formik
                initialValues={{
                    name: '',
                    password: '',
                    confirmPassword: '',
                }}
                validationSchema={signupSchema}
                validate={(values) => {
                    const errors: {
                        password?: string;
                        confirmPassword?: string;
                    } = {};

                    if (values.password !== values.confirmPassword) {
                        errors.confirmPassword = 'Passwords do not match.';
                    }

                    return errors;
                }}
                validateOnChange
                onSubmit={async (values) => {
                    try {
                        await axios.post('/API/Account/Signup/v1', {
                            name: values.name,
                            password: values.password,
                        });

                        localStorage.setItem('authenticated', 'true');
                        navigate('/');
                    } catch (err) {
                        if (axios.isAxiosError<APIError<object> | null>(err)) {
                            setError(err.response?.data);
                        } else {
                            setError({
                                errors: {},
                                message: 'Unknown Error',
                                status: 500,
                            });
                        }
                    }
                }}
            >
                {({ handleSubmit, handleChange, values, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Row className="mb-3">
                            <FormGroup as={Col} controlId="nameId">
                                <FormLabel>Name</FormLabel>
                                <InputGroup hasValidation>
                                    <FormControl
                                        autoFocus
                                        autoComplete="on"
                                        type="text"
                                        aria-describedby="inputGroupPrepend"
                                        name="name"
                                        value={values.name}
                                        onChange={handleChange}
                                        isInvalid={!!errors.name}
                                    />
                                    <FormControl.Feedback type="invalid">{errors.name}</FormControl.Feedback>
                                </InputGroup>
                            </FormGroup>
                        </Row>
                        <Row className="mb-3">
                            <FormGroup as={Col} controlId="PasswordID">
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
                        <Row className="mb-3">
                            <FormGroup as={Col} controlId="ConfirmPasswordID">
                                <FormLabel>Confirm Password</FormLabel>
                                <FormControl
                                    autoComplete="on"
                                    type="password"
                                    name="confirmPassword"
                                    value={values.confirmPassword}
                                    onChange={handleChange}
                                    isInvalid={!!errors.confirmPassword}
                                />
                                <FormControl.Feedback type="invalid">
                                    {errors.confirmPassword}
                                </FormControl.Feedback>
                            </FormGroup>
                        </Row>
                        <Row>
                            <Col className="d-flex justify-content-end">
                                <Button type="submit">Sign Up</Button>
                                <Button
                                    variant="secondary"
                                    className="ms-1"
                                    onClick={() => setShowSignin(!showSignin)}
                                >
                                    Sign In
                                </Button>
                            </Col>
                        </Row>
                    </Form>
                )}
            </Formik>
        </>
    );
}

function Signin({
    setError,
    setShowSignin,
    showSignin,
}: {
    setError: setState<APIError<object> | null>;
    setShowSignin: setState<boolean>;
    showSignin: boolean;
}) {
    const navigate = useNavigate();

    return (
        <>
            <Formik
                validationSchema={signinSchema}
                validateOnChange
                onSubmit={async (values) => {
                    try {
                        await axios.post('/API/Account/Signin/v1', {
                            name: values.name,
                            password: values.password,
                        });

                        localStorage.setItem('authenticated', 'true');
                        navigate('/');
                    } catch (err) {
                        if (axios.isAxiosError<APIError<object> | null>(err)) {
                            setError(err.response?.data);
                        } else {
                            setError({
                                title: 'Unknown Error',
                                detail: 'Something went wrong!',
                                status: 500,
                            });
                        }
                    }
                }}
                initialValues={{
                    name: '',
                    password: '',
                }}
            >
                {({ handleSubmit, handleChange, values, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Row className="mb-3">
                            <FormGroup as={Col} controlId="nameId">
                                <FormLabel>Name</FormLabel>
                                <InputGroup hasValidation>
                                    <FormControl
                                        autoComplete="on"
                                        autoFocus
                                        type="text"
                                        aria-describedby="inputGroupPrepend"
                                        name="name"
                                        value={values.name}
                                        onChange={handleChange}
                                        isInvalid={!!errors.name}
                                    />
                                    <FormControl.Feedback type="invalid">{errors.name}</FormControl.Feedback>
                                </InputGroup>
                            </FormGroup>
                        </Row>
                        <Row className="mb-3">
                            <FormGroup as={Col} controlId="PasswordID">
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
                        <Row>
                            <Col className="d-flex justify-content-end">
                                <Button type="submit">Sign In</Button>
                                <Button
                                    variant="secondary"
                                    className="ms-1"
                                    onClick={() => setShowSignin(!showSignin)}
                                >
                                    Sign Up
                                </Button>
                            </Col>
                        </Row>
                    </Form>
                )}
            </Formik>
        </>
    );
}
