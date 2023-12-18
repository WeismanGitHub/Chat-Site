import { Toast, ToastContainer } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import Navbar from '../components/navbar';
import Endpoints from '../endpoints';

type GetFriendRequesError = {
    type?: [string];
    page?: [string];
};

export default function Requests() {
    const [toastError, setToastError] = useState<APIErrorRes<GetFriendRequesError> | null>(null);
    const [requests, setRequests] = useState<FriendRequest[]>([]);
    const [total, setTotal] = useState<number | null>(null);
    const [showError, setShowError] = useState(false);
    const [type, SetType] = useState<'Incoming' | 'Outgoing'>('Incoming');
    const [page, setPage] = useState(1);

    useEffect(() => {
        Endpoints.Friends.Requests.get({ page, type })
            .then((res) => {
                setRequests(res?.friendRequests ?? []);
                setTotal(res?.totalCount ?? null);
            })
            .catch((err: APIErrorRes<object>) => {
                setToastError(err);
            });
    }, [type]);

    setPage;
    page;
    console.log(total, requests);

    return (
        <div>
            <Navbar />
            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowError(false)}
                    show={showError}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">
                            {toastError?.message || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {toastError?.errors &&
                            Object.values(toastError?.errors).map((err) => {
                                console.log(err);
                                return <div key={err[0]}>{err[0]}</div>;
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>
            <div className="text-center fs-3">
                <div
                    className="btn btn-outline-primary w-25 m-1"
                    onClick={() => SetType(type == 'Incoming' ? 'Outgoing' : 'Incoming')}
                >
                    {type}
                </div>
                <br />
                Total: {total}
                <br />
                <ul className="list-group w-75 m-auto">
                    {requests.map((req) => {
                        return (
                            <li
                                key={req.createdAt}
                                className="list-group-item bg-light-subtle text-primary border-secondary m-2"
                            >
                                {req.message}
                                <br />
                                {type === 'Outgoing' ? <div className="btn btn-danger">Delete</div> : <div></div>}
                            </li>
                        );
                    })}
                </ul>
            </div>
        </div>
    );
}
