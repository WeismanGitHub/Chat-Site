import { Button } from 'react-bootstrap';
import { useState } from 'react';

import Signin from '../components/signin';
import Signup from '../components/signup';

export default function Auth() {
    const [showSignin, setShowSignin] = useState<boolean>(true);

    return (
        <div className='container'>
            <div className='row vh-100 align-items-center justify-content-center'>
                <div className='col-sm-8 col-md-6 col-lg-4 bg-white rounded p-4 shadow'>
                    {showSignin ? <Signin /> : <Signup />}
                    <Button className='btn-secondary mt-1 bg-bg-secondary-subtle' onClick={() => setShowSignin(!showSignin)} >
                        Click here to {showSignin ? 'signup' : 'signin'}.
                    </Button>
                </div>
            </div>
        </div>
    );
}
