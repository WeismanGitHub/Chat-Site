import { ToastContainer, Toast } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../endpoints';
import ky, { HTTPError } from 'ky';
import { useState } from 'react';

type friend = {
    id: string;
    displayName: string;
    createdAt: string;
};

export default function Friends() {
    const toggleError = () => setShowError(!showError);
    const [showError, setShowError] = useState(false);

    const { error, data } = useQuery<friend[], HTTPError>({
        queryKey: ['data'],
        queryFn: (): Promise<friend[]> => ky.get(Endpoints.Friends).json(),
    });

    if (error) {
        setShowError(true);
    }

    return (
        <div className="">
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

            <ul className="list-group fs-5">
                {data &&
                    data!
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .concat(data)
                        .map((friend) => {
                            return (
                                <li
                                    className="list-group-item bg-dark-subtle text-primary border-secondary"
                                    key={friend.id}
                                >
                                    {friend.displayName}
                                    <div className="fs-6">
                                        Created -{' '}
                                        {new Date(
                                            friend.createdAt
                                        ).toLocaleDateString('en-US', {
                                            weekday: 'long',
                                            year: 'numeric',
                                            month: 'long',
                                            day: 'numeric',
                                        })}
                                    </div>
                                </li>
                            );
                        })}
            </ul>
        </div>
    );
}
