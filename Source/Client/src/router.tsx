import { createBrowserRouter } from 'react-router-dom';

import Auth from './pages/auth.tsx';
import Home from './pages/home.tsx';

const router = createBrowserRouter([
    { path: '/Auth', element: <Auth /> },
    { path: '/', element: <Home /> },
]);

export default router;
