import { Form, Button, Col, InputGroup } from 'react-bootstrap';
import * as formik from 'formik';
import * as yup from 'yup';

export default function Signup() {
    const { Formik } = formik;

    const schema = yup.object().shape({
        dislayName: yup.string().required(),
        email: yup.string().required().email(),
        password: yup.string().required(),
        confirmPassword: yup.string().required(),
    });

    // function submit(data: typeof schema) {
    //     console.log(data)
    // }

    return (
        <Formik
            validationSchema={schema}
            onSubmit={console.log}
            initialValues={{
                displayName: 'Display Name',
                email: 'example@email.com',
                password: 'Password',
                confirmPassword: 'Password',
            }}
        >
            {({ handleSubmit, handleChange, values, touched, errors }) => (
                <Form noValidate onSubmit={handleSubmit}>
                    <Form.Group
                        as={Col}
                        md="4"
                        controlId="validationFormikDisplayName"
                    >
                        <Form.Label>Display Name</Form.Label>
                        <Form.Control
                            type="text"
                            name="DisplayName"
                            placeholder={values.displayName}
                            onChange={handleChange}
                            isValid={touched.displayName && !errors.displayName}
                        />
                        <Form.Control.Feedback>
                            Looks good!
                        </Form.Control.Feedback>
                    </Form.Group>
                    <Form.Group
                        as={Col}
                        md="4"
                        controlId="validationFormikEmail"
                    >
                        <Form.Label>Email</Form.Label>
                        <InputGroup hasValidation>
                            <Form.Control
                                type="text"
                                placeholder="Email"
                                aria-describedby="inputGroupPrepend"
                                name="email"
                                onChange={handleChange}
                                isInvalid={!!errors.email}
                            />
                            <Form.Control.Feedback type="invalid">
                                {errors.email}
                            </Form.Control.Feedback>
                        </InputGroup>
                    </Form.Group>
                        <Form.Group
                            as={Col}
                            md="3"
                            controlId="validationFormik04"
                        >
                            <Form.Label>Password</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder={values.password}
                                name="password"
                                onChange={handleChange}
                                isInvalid={!!errors.password}
                            />
                            <Form.Control.Feedback type="invalid">
                                {errors.password}
                            </Form.Control.Feedback>
                        </Form.Group>
                        <Form.Group
                            as={Col}
                            md="3"
                            controlId="validationFormik05"
                        >
                            <Form.Label>Confirm Password</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder={values.confirmPassword}
                                name="confirm password"
                                onChange={handleChange}
                                isInvalid={!!errors.confirmPassword}
                            />

                            <Form.Control.Feedback type="invalid">
                                {errors.confirmPassword}
                            </Form.Control.Feedback>
                        </Form.Group>
                    <Button type="submit">Submit form</Button>
                </Form>
            )}
        </Formik>
    );
}
