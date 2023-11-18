import { Button, Col, Form, InputGroup, Row } from 'react-bootstrap';
import * as formik from 'formik';
import * as yup from 'yup';

export default function Signin() {
    const { Formik } = formik;

    const schema = yup.object().shape({
        email: yup.string().required().email("Must be a valid email."),
        password: yup.string().required().min(10).max(70),
    });

  return (<Formik
    validationSchema={schema}
    validateOnChange
        onSubmit={(values) => {
            console.log(values)
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
    </Formik>);
}