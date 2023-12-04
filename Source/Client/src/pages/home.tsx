import { redirectIfNotLoggedIn } from '../helpers';

import Conversations from '../components/conversations';
import CreateConvo from '../components/create-convo';
import AddFriend from '../components/add-friend';
import JoinConvo from '../components/join-convo';
import Friends from '../components/friends';
import Navbar from '../components/navbar';

export default function Home() {
    redirectIfNotLoggedIn();

    return (
        <div className="overflow-y-hidden vh-100 vw-100">
            <Navbar />

            <div className="w-25 col m-1 text-center h-100">
                <div className="row justify-content-evenly">
                    <div className="p-1">
                        <AddFriend /> <CreateConvo /> <JoinConvo />
                    </div>
                </div>
                <div className="overflow-y-scroll h-100">
                    <Friends />
                </div>
                <div className="overflow-y-scroll h-100">
                    <Conversations />
                </div>
            </div>
        </div>
    );
}
