import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

function redirectIfNotLoggedIn() { // I know this is kinda unclean.
    const loggedIn = localStorage.getItem('loggedIn');
    const navigate = useNavigate();

    useEffect(() => {
        if (!loggedIn) {
            navigate('/auth');
        }
    }, []);
}

export {
    redirectIfNotLoggedIn
}