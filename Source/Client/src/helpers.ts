import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

function redirectIfNotLoggedIn() {
    // I know this is kinda unclean.
    const authenticated = localStorage.getItem('authenticated');
    const navigate = useNavigate();

    useEffect(() => {
        if (!authenticated) {
            navigate('/auth');
        }
    }, []);
}

export { redirectIfNotLoggedIn };
