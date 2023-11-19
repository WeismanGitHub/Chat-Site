import { ToastContainer, Toast } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Endpoints from '../endpoints';
import ky, { HTTPError } from 'ky';
import { useState } from 'react';

export default function Navbar() {
    const loggedIn = Boolean(localStorage.getItem('loggedIn'));
    const navigate = useNavigate();

    const [showError, setShowError] = useState(false);
    const [error, setError] = useState<HTTPError>();
    const toggleError = () => setShowError(!showError);

    async function logout() {
        await ky
            .post(Endpoints.Signout)
            .then(() => {
                localStorage.removeItem('loggedIn');
                navigate('/auth');
            })
            .catch((err: HTTPError) => {
                setShowError(true);
                setError(err);
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
                        <strong className="me-auto">
                            {error?.name || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {error?.message || 'Unable to read error message.'}
                    </Toast.Body>
                </Toast>
            </ToastContainer>
            <nav className="navbar navbar-expand navbar-dark bg-primary ps-2 pe-2 justify-content-center py-1">
                <a className="navbar-brand" href="/">
                    <img
                        src="/icon.png"
                        width={50}
                        height={50}
                        alt="icon"
                        className="me-2"
                    />
                    Chat Site v2
                </a>

                <div className="justify-content-start navbar-nav">
                    <a className="nav-item nav-link active" href="/">
                        Home
                    </a>
                    <a className="nav-item nav-link active" href="/about">
                        About
                    </a>
                    <div className="ms-10">
                        {loggedIn ? (
                            <button
                                className="nav-item nav-link active"
                                onClick={() => logout()}
                            >
                                Logout
                            </button>
                        ) : (
                            <a
                                className="nav-item nav-link active"
                                href="/auth"
                            >
                                Signup/Signin
                            </a>
                        )}
                    </div>
                </div>
            </nav>
        </>
    );
}
