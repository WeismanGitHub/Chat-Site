import { redirectIfNotLoggedIn } from '../helpers';

import Conversations from '../components/home/conversations';
import CreateConvo from '../components/home/create-convo';
import AddFriend from '../components/home/add-friend';
import JoinConvo from '../components/home/join-convo';
import Friends from '../components/home/friends';
import Navbar from '../components/navbar';
import { useState } from 'react';

export default function Home() {
    redirectIfNotLoggedIn();

    const [convoID, setConvoID] = useState<string | null>(null);
    const [conversations, setConversations] = useState<ConversationsData>([]);
    const [toggle, setToggle] = useState(true);
    convoID

    return (
        <div className="overflow-y-hidden vh-100 vw-100">
            <Navbar />
            <div className="row vh-100 " style={{ width: '20%' }}>
                <div className="w-100 text-center">
                    <div className="btn btn-outline-primary w-50 m-1" onClick={() => setToggle(!toggle)}>
                        Show {toggle ? 'Friends' : 'Convos'}
                    </div>
                </div>
                {toggle ? (
                    <div className="col text-center h-100 m-1">
                        <div className="row justify-content-evenly mb-1">
                            <div>
                                <CreateConvo
                                    setConversations={setConversations}
                                    conversations={conversations}
                                />{' '}
                                <JoinConvo setConversations={setConversations} />
                            </div>
                        </div>
                        <div className="overflow-y-scroll h-100">
                            <Conversations
                                conversations={conversations}
                                setConvoID={setConvoID}
                                setConversations={setConversations}
                            />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </div>
                    </div>
                ) : (
                    <div className="col text-center h-100 m-1">
                        <div className="row justify-content-evenly mb-1">
                            <div>
                                <AddFriend />
                            </div>
                        </div>
                        <div className="overflow-y-scroll h-100">
                            <Friends />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
