import { Toast, ToastContainer } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

type GetFriendRequesError = {
    type?: string[];
    page?: string[];
};

export default function IncomingRequests() {
    const { data, error } = useQuery<{ friendRequests: FriendRequest[]; totalCount: number }, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Friends.Requests.get({}),
    });

    const [toastError, setToastError] = useState<APIErrorRes<GetFriendRequesError> | null>(null);
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setShowError(true);
                setToastError(res);
            });
        }
    }, [error]);

    return (
        <div>
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
                            Object.values(toastError?.errors)?.map((err) => {
                                return <div key={err?.[0]}>{err?.[0]}</div>;
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            {data?.totalCount}
        </div>
    );
}
