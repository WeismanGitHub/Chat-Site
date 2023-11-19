import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Friends from '../components/friends';
import Navbar from '../components/navbar';

export default function Home() {
    const loggedIn = localStorage.getItem('loggedIn');
    const navigate = useNavigate();

    useEffect(() => {
        if (!loggedIn) {
            navigate('/auth');
        }
    }, [])
    
    return (
        <div>
            <Navbar />
            <div className='w-25 row vh-100 m-1 text-center'>
                <Friends />
            </div>
        </div>
    );
}
