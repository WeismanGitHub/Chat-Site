import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

import CreateConvo from '../components/create-convo';
import AddFriend from '../components/add-friend';
import Friends from '../components/friends';
import Navbar from '../components/navbar';

export default function Home() {
    const loggedIn = localStorage.getItem('loggedIn');
    const navigate = useNavigate();

    useEffect(() => {
        if (!loggedIn) {
            navigate('/auth');
        }
    }, []);

    return (
        <div className="overflow-y-hidden vh-100 vw-100">
            <Navbar />

            <div className="w-25 col m-1 text-center h-100">
                <div className="row">
                    <AddFriend />
                    <CreateConvo />
                </div>
                <div className="overflow-y-scroll h-100">
                    <Friends />
                </div>
            </div>
        </div>
    );
}
