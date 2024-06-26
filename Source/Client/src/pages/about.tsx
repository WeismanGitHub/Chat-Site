/* eslint-disable react/no-unescaped-entities */
import { Button, Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import Navbar from '../navbar';

export default function About() {
    return (
        <>
            <Navbar />
            <div className="full-height-minus-navbar d-flex align-items-center justify-content-center">
                <Card style={{ width: '80%', maxWidth: '500px' }}>
                    <Card.Header className="bg-primary text-white">
                        <h2>About Chat Site v2</h2>
                    </Card.Header>
                    <Card.Body className="fs-4">
                        <Card.Text>
                            <p>
                                Chat Site v2 is a remake of an{' '}
                                <Link
                                    to="https://github.com/WeismanGitHub/Chat-Website"
                                    className="link-underline-primary"
                                >
                                    older project
                                </Link>
                                . This time, I've chosen to use <strong>ASP.NET</strong> and the{' '}
                                <strong>FastEndpoints</strong> library for my back-end and{' '}
                                <strong>Typescript, React, and Bootstrap</strong> for the front-end. By
                                revisiting and enhancing my prior work, I reinforce my existing knowledge but
                                also push the boundaries of what I can achieve.
                            </p>
                            <div className="d-flex justify-content-center">
                                <a className="m-1" href={'https://github.com/WeismanGitHub/Chat-Site-v2'}>
                                    <Button className="btn-lg">GitHub</Button>
                                </a>
                                <a className="m-1" href={'/swagger/index.html'}>
                                    <Button className="btn-lg">Swagger</Button>
                                </a>
                            </div>
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        </>
    );
}
