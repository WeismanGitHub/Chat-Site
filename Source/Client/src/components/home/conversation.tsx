import { useQuery } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

type GetOneError = object;

export default function conversation({ conversationID }: { conversationID: string }) {
    const { error, data } = useQuery<SingleConvoData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.getOne(conversationID),
    });

    const [toastError, setToastError] = useState<APIErrorRes<GetOneError> | null>(null);
    const [showError, setShowError] = useState(false);
    data;
    showError;
    toastError;

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setShowError(true);
                setToastError(res);
            });
        }
    }, [error]);

    return <>sdfsfs</>;
}
