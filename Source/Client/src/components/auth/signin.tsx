import { Button, Col, Form, InputGroup, Row, Toast, ToastContainer } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Endpoints from '../../endpoints';
import * as formik from 'formik';
import { useState } from 'react';
import { HTTPError } from 'ky';
import * as yup from 'yup';

type SigninError = {
    email?: string;
    password?: string;
};

export default function Signin() {
    const navigate = useNavigate();
    const { Formik } = formik;

    const schema = yup.object().shape({
        email: yup.string().required('Email is a required field.').email('Must be a valid email.'),
        password: yup
            .string()
            .required('Password is a required field.')
            .min(10, 'Must be at least 10 characters.')
            .max(70, 'Cannot be more than 70 characters.'),
    });

    const [showError, setShowError] = useState(false);
    const [error, setError] = useState<APIErrorRes<SigninError> | null>(null);
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

            <Formik
                validationSchema={schema}
                validateOnChange
                onSubmit={async (values) => {
                    await Endpoints.Account.signin(values)
                        .then(() => {
                            localStorage.setItem('loggedIn', 'true');
                            navigate('/');
                        })
                        .catch(async (err: HTTPError) => {
                            const res: APIErrorRes<SigninError> = await err.response.json();
                            setError(res);
                            setShowError(true);
                        });
                }}
                initialValues={{
                    email: '',
                    password: '',
                }}
            >
                {({ handleSubmit, handleChange, values, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
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
                        <Button type="submit">Sign In</Button>
                    </Form>
                )}
            </Formik>
        </>
    );
}
