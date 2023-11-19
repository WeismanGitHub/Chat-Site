import {
    Button,
    Col,
    Form,
    InputGroup,
    Row,
    Toast,
    ToastContainer,
} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Endpoints from '../endpoints';
import * as formik from 'formik';
import { useState } from 'react';
import * as yup from 'yup';
import ky, { HTTPError } from 'ky';

export default function Signin() {
    const navigate = useNavigate();
    const { Formik } = formik;

    const schema = yup.object().shape({
        email: yup.string().required().email('Must be a valid email.'),
        password: yup.string().required().min(10).max(70),
    });

    const [showError, setShowError] = useState(false);
    const [error, setError] = useState<HTTPError>();
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
                            {error?.name || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {error?.message || 'Unable to read error message.'}
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            <Formik
                validationSchema={schema}
                validateOnChange
                onSubmit={async (values) => {
                    await ky
                        .post(Endpoints.Signin, { json: values })
                        .then(() => {
                            localStorage.setItem('loggedIn', 'true');
                            navigate('/');
                        })
                        .catch((err: HTTPError) => {
                            setShowError(true);
                            setError(err);
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
                                        placeholder="example@email.com"
                                        name="email"
                                        value={values.email}
                                        onChange={handleChange}
                                        isInvalid={!!errors.email}
                                    />
                                </InputGroup>
                                <Form.Control.Feedback type="invalid">
                                    {errors.email}
                                </Form.Control.Feedback>
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
