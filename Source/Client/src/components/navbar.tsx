import { ToastContainer, Toast, Modal, Button } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import Endpoints from '../endpoints';
import { useState } from 'react';
import axios from 'axios';

type LogoutError = {
    email?: string;
    password?: string;
};

export default function Navbar() {
    const loggedIn = Boolean(localStorage.getItem('loggedIn'));
    const navigate = useNavigate();

    const [showError, setShowError] = useState(false);
    const [error, setError] = useState<APIErrorRes<LogoutError> | null>(null);
    const toggleError = () => setShowError(!showError);
    const [showModal, setShowModal] = useState(false);

    async function logout() {
        await Endpoints.Account.signout()
            .then(() => {
                localStorage.removeItem('loggedIn');
                navigate('/auth');
            })
            .catch(async (err) => {
                if (axios.isAxiosError<APIErrorRes<LogoutError>>(err) && err.response?.data) {
                    setError(err.response.data);
                    setShowError(true);
                }
            });
    }

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

            <Modal
                show={showModal}
                centered
                keyboard={true}
                onHide={() => setShowModal(false)}
                animation={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Are you sure you want to log out?</Modal.Title>
                </Modal.Header>
                <Modal.Footer>
                    <Button variant="danger" onClick={logout}>
                        Log Out
                    </Button>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>

            <nav className="navbar navbar-expand-lg navbar-expand-md navbar-dark bg-primary fixed-top p-1 ps-3 pe-3" id="navbar">
                <Link className="navbar-brand m-0" to={'/'}>
                    <img src="/icon.png" width={50} height={50} alt="icon" className="me-2" />
                    <span className="fs-3">Chat Site v2</span>
                </Link>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarResponsive"
                    aria-controls="navbarResponsive"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse justify-content-end" id="navbarResponsive">
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <Link className="nav-link" to={'/'}>
                                Home
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to={'/about'}>
                                About
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to={'/account'}>
                                Account
                            </Link>
                        </li>
                        {loggedIn && 
                            <li className="nav-item">
                            <Link className="nav-link" to={'/requests'}>
                                Requests
                            </Link>
                        </li>}
                        <li className="nav-item">
                            {loggedIn ? (
                                <div
                                    className="nav-link"
                                    style={{ cursor: 'pointer' }}
                                    onClick={() => setShowModal(true)}
                                >
                                    Log Out
                                </div>
                            ) : (
                                <Link className="nav-link" to="/Auth">
                                    Login
                                </Link>
                            )}
                        </li>
                    </ul>
                </div>
            </nav>
        </>
    );
}
