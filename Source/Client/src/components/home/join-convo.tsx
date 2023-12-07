import { Dispatch, SetStateAction } from 'react';

export default function JoinConvo({
    setConversations,
}: {
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
}) {
    setConversations;
    return <div className="btn btn-outline-primary">Join Convo</div>;
}
