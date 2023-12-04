import { ToastContainer, Toast } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Endpoints from '../endpoints';
import { useState } from 'react';
import { HTTPError } from 'ky';

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

    async function logout() {
        await Endpoints.Account.signout()
            .then(() => {
                localStorage.removeItem('loggedIn');
                navigate('/auth');
            })
            .catch(async (err: HTTPError) => {
                const res: APIErrorRes<LogoutError> = await err.response.json();
                setError(res);
                setShowError(true);
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
            <nav className="navbar navbar-expand navbar-dark bg-primary ps-2 pe-2 justify-content-center py-1">
                <a className="navbar-brand" href="/">
                    <img src="/icon.png" width={50} height={50} alt="icon" className="me-2" />
                    Chat Site v2
                </a>

                <div className="justify-content-start navbar-nav">
                    <a className="nav-item nav-link active" href="/">
                        Home
                    </a>
                    {loggedIn && (
                        <a className="nav-item nav-link active" href="/account">
                            Account
                        </a>
                    )}
                    <a className="nav-item nav-link active" href="/about">
                        About
                    </a>
                    <div className="ms-10">
                        {loggedIn ? (
                            <button className="nav-item nav-link active" onClick={() => logout()}>
                                Logout
                            </button>
                        ) : (
                            <a className="nav-item nav-link active" href="/auth">
                                Signup/Signin
                            </a>
                        )}
                    </div>
                </div>
            </nav>
        </>
    );
}
