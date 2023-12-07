import { Dispatch, SetStateAction } from 'react';

export default function CreateConvo({
    setConversations,
}: {
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
}) {
    setConversations;
    return <div className="btn btn-outline-primary">Create Convo</div>;
}
