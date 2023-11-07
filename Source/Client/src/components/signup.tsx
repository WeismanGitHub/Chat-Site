import { Form, Button, Col, InputGroup } from 'react-bootstrap';
import { useState } from 'react';

export default function Signup() {
    const [validated, setValidated] = useState(false);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        const form = event.currentTarget;

        if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
        }

        setValidated(true);
    };

    return (
        <Form noValidate validated={validated} onSubmit={handleSubmit}>
            <Form.Group as={Col} md="4" controlId="validationCustom01">
                <Form.Label>Display Name</Form.Label>
                <Form.Control
                    required
                    type="text"
                    placeholder="DisplayName"
                />
                <Form.Control.Feedback>Looks good!</Form.Control.Feedback>
            </Form.Group>
            <Form.Group
                as={Col}
                md="4"
                controlId="validationCustomEmail"
            >
                <Form.Label>Email</Form.Label>
                <InputGroup hasValidation>
                    <Form.Control
                        type="text"
                        placeholder="example@email.com"
                        aria-describedby="inputGroupPrepend"
                        required
                    />
                    <Form.Control.Feedback type="invalid">
                        Please choose a valid email.
                    </Form.Control.Feedback>
                </InputGroup>
            </Form.Group>
            <Form.Group
                as={Col}
                md="4"
                controlId="validationCustomPassword"
            >
                <Form.Label>Password</Form.Label>
                <InputGroup hasValidation>
                    <Form.Control
                        type="password"
                        placeholder="Password"
                        aria-describedby="inputGroupPrepend"
                        required
                    />
                    <Form.Control.Feedback type="invalid">
                        Please choose a password between 10-70 characters, at least one digit, lowercase letter, and uppercase letter.
                    </Form.Control.Feedback>
                </InputGroup>
            </Form.Group>
            <Form.Group
                as={Col}
                md="4"
                controlId="validationCustomReenterPassword"
            >
                <Form.Label>Re-enter Password</Form.Label>
                <InputGroup hasValidation>
                    <Form.Control
                        type="password"
                        placeholder="Password"
                        aria-describedby="inputGroupPrepend"
                        required
                    />
                    <Form.Control.Feedback type="invalid">
                        Please re-enter your password.
                    </Form.Control.Feedback>
                </InputGroup>
            </Form.Group>
            <Button type="submit">Submit form</Button>
        </Form>
    );
}
