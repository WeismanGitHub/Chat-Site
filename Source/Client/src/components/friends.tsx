import { ToastContainer, Toast } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../endpoints';
import ky, { HTTPError } from 'ky';
import { useState } from 'react';

type friend = {
    ID: string;
    DisplayName: string;
    CreatedAt: string;
};

export default function Friends() {
    const toggleError = () => setShowError(!showError);
    const [showError, setShowError] = useState(false);

    const { error, data } = useQuery<friend[], HTTPError>({
        queryKey: ['data'],
        queryFn: (): Promise<friend[]> => ky.get(Endpoints.Friends).json(),
    });

    if (error) {
        setShowError(true)
    }

    return <>
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

        <ul>
            {data && data!.map(friend => {
                return <li key={friend.ID}>
                    {friend.DisplayName}
                </li>
            })}
        </ul>
    </>
}
