import { Button, Col, Form, InputGroup, Row } from 'react-bootstrap';
import * as formik from 'formik';
import * as yup from 'yup';

export default function Signup() {
    const { Formik } = formik;

    const schema = yup.object().shape({
        displayName: yup.string().required().min(1).max(25),
        email: yup.string().required().email("Must be a valid email."),
        password: yup.string().required().min(10).max(70),
        confirmPassword: yup.string().required(),
    });

  return (<Formik
    validationSchema={schema}
    validate={(values) => {
        const errors: { confirmPassword?: string, } = {};

        if (values.password !== values.confirmPassword) {
            errors.confirmPassword = "Passwords do not match."
        }

        return errors
    }}
    validateOnChange
        onSubmit={(values) => {
            console.log(values)
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
    </Formik>);
}