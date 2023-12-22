import { Pagination, Toast, ToastContainer } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import Navbar from '../components/navbar';
import Endpoints from '../endpoints';

type GetFriendRequesError = {
    type?: [string];
    page?: [string];
};

export default function Requests() {
    const [toastError, setToastError] = useState<APIErrorRes<GetFriendRequesError> | null>(null);
    const [type, SetType] = useState<'Incoming' | 'Outgoing'>('Incoming');
    const [requests, setRequests] = useState<FriendRequest[]>([]);
    const [total, setTotal] = useState<number | null>(null);
    const [showSuccess, setShowSuccess] = useState(false);
    const [showError, setShowError] = useState(false);
    const [page, setPage] = useState(1);

    useEffect(() => {
        Endpoints.Friends.Requests.get({ page, type })
            .then((res) => {
                setRequests(res?.friendRequests ?? []);
                setTotal(res?.totalCount ?? null);
            })
            .catch((err: APIErrorRes<object>) => {
                setToastError(err);
                setShowError(true);
            });
    }, [type, page]);

    async function deleteRequest(id: string) {
        Endpoints.Friends.Requests.delete(id)
            .then(() => {
                setRequests(requests.filter((r) => r.id !== id));
                setTotal(total! - 1);
                setShowSuccess(true);
            })
            .catch((err: APIErrorRes<object>) => {
                setToastError(err);
                setShowError(true);
            });
    }

    async function declineRequest(id: string) {
        Endpoints.Friends.Requests.decline(id)
            .then(() => setShowSuccess(true))
            .catch((err: APIErrorRes<object>) => {
                setToastError(err);
                setShowError(true);
            });
    }

    async function acceptRequest(id: string) {
        Endpoints.Friends.Requests.accept(id)
            .then(() => setShowSuccess(true))
            .catch((err: APIErrorRes<object>) => {
                setToastError(err);
                setShowError(true);
            });
    }

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
            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowSuccess(false)}
                    show={showSuccess}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'success'}
                >
                    <Toast.Header>
                        <strong className="me-auto">Success!</strong>
                    </Toast.Header>
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
                <div className="w-75 m-auto">
                    <ul className="list-group">
                        {requests.map((req) => {
                            return (
                                <li
                                    key={req.createdAt}
                                    className="list-group-item bg-light-subtle text-primary border-secondary m-2"
                                >
                                    <div className="fs-4">
                                        Status: {['Accepted', 'Declined', 'Pending'][req.status]}
                                    </div>
                                    {req.message}
                                    <br />
                                    {(type === 'Incoming' && req.status === 2) && (
                                        <div>
                                            <div
                                                className="btn btn-success me-1"
                                                onClick={() => acceptRequest(req.id)}
                                            >
                                                Accept
                                            </div>
                                            <div
                                                className="btn btn-danger"
                                                onClick={() => declineRequest(req.id)}
                                            >
                                                Decline
                                            </div>
                                        </div>
                                    )}
                                    {type === 'Outgoing' && (
                                        <div className="btn btn-danger" onClick={() => deleteRequest(req.id)}>
                                            Delete
                                        </div>
                                    )}
                                </li>
                            );
                        })}
                    </ul>
                    <Pagination size="lg" className="justify-content-center">
                        <Pagination.First disabled={page === 1} onClick={() => setPage(1)} />
                        <Pagination.Prev disabled={page === 1} onClick={() => setPage(page - 1)} />
                        <Pagination.Item>{page}</Pagination.Item>
                        <Pagination.Next disabled={requests.length < 10} onClick={() => setPage(page + 1)} />
                        <Pagination.Last
                            disabled={requests.length < 10}
                            onClick={() => setPage(Math.floor(total! / 10) + 1)}
                        />
                    </Pagination>
                </div>
            </div>
        </div>
    );
}
