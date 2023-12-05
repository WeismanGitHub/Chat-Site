import { createBrowserRouter } from 'react-router-dom';

import Requests from './pages/requests.tsx';
import Account from './pages/account.tsx';
import About from './pages/about.tsx';
import Auth from './pages/auth.tsx';
import Home from './pages/home.tsx';

const router = createBrowserRouter([
    { path: '/Requests', element: <Requests /> },
    { path: '/Account', element: <Account /> },
    { path: '/About', element: <About /> },
    { path: '/Auth', element: <Auth /> },
    { path: '/', element: <Home /> },
]);

export default router;
