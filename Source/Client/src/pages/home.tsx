import { redirectIfNotLoggedIn } from '../helpers';
import { useState } from 'react';

import Conversations from '../components/home/conversations';
import Conversation from '../components/home/conversation';
import CreateConvo from '../components/home/create-convo';
import AddFriend from '../components/home/add-friend';
import JoinConvo from '../components/home/join-convo';
import Friends from '../components/home/friends';
import Navbar from '../components/navbar';

export default function Home() {
    redirectIfNotLoggedIn();

    const [convoID, setConvoID] = useState<string | null>(null);
    const [toggle, setToggle] = useState(true);

    return (
        <div className="vh-100 vw-100 overflow-x-hidden overflow-y-hidden">
            <Navbar />
            <div className="row text-center">
                <div className="col-3 text-center d-flex flex-column">
                    <div className="w-100">
                        <div className="btn btn-outline-primary w-50 m-1" onClick={() => setToggle(!toggle)}>
                            Show {toggle ? 'Friends' : 'Convos'}
                        </div>
                    </div>
                    {toggle ? (
                        <div className="row justify-content-evenly mb-1">
                            <div>
                                <CreateConvo /> <JoinConvo />
                            </div>
                        </div>
                    ) : (
                        <div className="row justify-content-evenly mb-1">
                            <div>
                                <AddFriend />
                            </div>
                        </div>
                    )}
                    <div className="overflow-y-scroll h-25 min-vh-100 pb-5">
                        <div className="mb-3">
                            {toggle ? <Conversations setConvoID={setConvoID} /> : <Friends />}
                            <br />
                            <br />
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
                <div className="col-8">
                    {convoID ? (
                        <Conversation conversationID={convoID} />
                    ) : (
                        <div className="fs-2">
                            <br />
                            <br />
                            No Convo Opened
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
