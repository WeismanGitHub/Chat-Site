import { useNavigate } from 'react-router-dom';
import Endpoints from '../endpoints';
import ky, { HTTPError } from 'ky';
import * as formik from 'formik';
import { useState } from 'react';
import * as yup from 'yup';
import {
    Button,
    Col,
    Form,
    InputGroup,
    Row,
    Toast,
    ToastContainer,
} from 'react-bootstrap';

type SignupError = {
    email?: string;
    password?: string;
    displayName?: string;
};

export default function Signup() {
    const navigate = useNavigate();
    const { Formik } = formik;

    const schema = yup.object().shape({
        displayName: yup
            .string()
            .required('DisplayName is a required field.')
            .min(1, 'Must be at least 1 character.')
            .max(25, 'Cannot be more than 25 characters.'),
        email: yup
            .string()
            .required('Email is a required field.')
            .email('Must be a valid email.'),
        password: yup
            .string()
            .required('Password is a required field.')
            .min(10, 'Must be at least 10 characters.')
            .max(70, 'Cannot be more than 70 characters.'),
        confirmPassword: yup
            .string()
            .required('Confirm Password is a required field.'),
    });

    const [showError, setShowError] = useState(false);
    const [error, setError] = useState<APIErrorRes<SignupError> | null>(null);
    const toggleError = () => setShowError(!showError);

    return (
        <>
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
                            {error?.message || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {error?.errors &&
                            Object.values(error?.errors).map((err) => {
                                return (
                                    <div key={err.toString()}>
                                        {err.toString()}
                                    </div>
                                );
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            <Formik
                validationSchema={schema}
                validate={(values) => {
                    const errors: {
                        password?: string;
                        confirmPassword?: string;
                    } = {};

                    if (values.password !== values.confirmPassword) {
                        errors.confirmPassword = 'Passwords do not match.';
                    }

                    let hasUpperCase = false;
                    let hasLowerCase = false;
                    let hasDigit = false;

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

                    return errors;
                }}
                validateOnChange
                onSubmit={async (values) => {
                    await ky
                        .post(Endpoints.Account.Signup(), {
                            json: {
                                DisplayName: values.displayName,
                                Email: values.email,
                                Password: values.password,
                            },
                        })
                        .then(() => {
                            localStorage.setItem('loggedIn', 'true');
                            navigate('/');
                        })
                        .catch(async (err: HTTPError) => {
                            const res: APIErrorRes<SignupError> =
                                await err.response.json();
                            setError(res);
                            setShowError(true);
                        });
                }}
                initialValues={{
                    displayName: '',
                    email: '',
                    password: '',
                    confirmPassword: '',
                }}
            >
                {({ handleSubmit, handleChange, values, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Row className="mb-3">
                            <Form.Group as={Col} controlId="DisplayNameID">
                                <Form.Label>DisplayName</Form.Label>
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
                                <Form.Label>Password</Form.Label>
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
                            <Form.Group as={Col} controlId="ConfirmPasswordID">
                                <Form.Label>Confirm</Form.Label>
                                <Form.Control
                                    type="password"
                                    name="confirmPassword"
                                    value={values.confirmPassword}
                                    onChange={handleChange}
                                    isInvalid={!!errors.confirmPassword}
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
        </>
    );
}
