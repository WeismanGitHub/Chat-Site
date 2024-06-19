import { redirectIfNotLoggedIn } from '../helpers';
import { useState } from 'react';

import Conversations from '../components/home/conversations';
import Conversation from '../components/home/conversation';
import CreateConvo from '../components/home/create-convo';
import JoinConvo from '../components/home/join-convo';
import Navbar from '../components/navbar';

export default function Home() {
    redirectIfNotLoggedIn();

    const [convoID, setConvoID] = useState<string | null>(null);

    return (
        <>
            <Navbar />
            <div className="full-height-minus-navbar">
                <div className="col-3 text-center d-flex flex-column">
                    <div className="row justify-content-evenly mb-1">
                        <CreateConvo /> <JoinConvo />
                    </div>
                    {<Conversations setConvoID={setConvoID} />}
                </div>
                <div className="col-9">
                    {convoID ? (
                        <Conversation conversationID={convoID} />
                    ) : (
                        <div className="fs-3">
                            <br />
                            <br />
                            No Chat Opened
                        </div>
                    )}
                </div>
            </div>
        </>
    );
}
