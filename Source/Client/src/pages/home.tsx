import { redirectIfNotLoggedIn } from '../helpers';

import Conversations from '../components/home/conversations';
import CreateConvo from '../components/home/create-convo';
import AddFriend from '../components/home/add-friend';
import JoinConvo from '../components/home/join-convo';
import Friends from '../components/home/friends';
import Navbar from '../components/navbar';

export default function Home() {
    redirectIfNotLoggedIn();

    return (
        <div className="overflow-y-hidden vh-100 vw-100">
            <Navbar />

            <div className="col text-center h-100 m-1" style={{ width: '20%' }}>
                <div className="row justify-content-evenly mb-1">
                    <div>
                        <AddFriend />
                    </div>
                </div>
                <div className="overflow-y-scroll h-100">
                    <Friends />
                </div>
            </div>
            <div className="col text-center h-100" style={{ width: '20%' }}>
                <div className="row justify-content-evenly m-1">
                    <div>
                        <CreateConvo /> <JoinConvo />
                    </div>
                </div>
                <div className="overflow-y-scroll h-100">
                    <Conversations />
                </div>
            </div>
        </div>
    );
}
