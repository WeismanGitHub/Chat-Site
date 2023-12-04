import { useQuery } from '@tanstack/react-query';
import Endpoints from '../endpoints';
import { HTTPError } from 'ky';

export default function conversation({ conversationID }: { conversationID: string }) {
    const { error, data } = useQuery<SingleConvoData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.getOne(conversationID),
    });

    if (error) {
        // stuff
    }

    return <>{data}</>;
}
