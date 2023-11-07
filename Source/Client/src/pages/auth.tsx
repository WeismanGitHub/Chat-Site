import { useState } from 'react';

import Signin from '../components/signin';
import Signup from '../components/signup';

export default function Auth() {
    const [showSignin, setShowSignin] = useState<boolean>(true);

    return (
        <div>
            <div className="" style={{ margin: 'auto' }}>
                <div className="" style={{ margin: 'auto' }}>
                    {showSignin ? <Signin /> : <Signup />}
                    <button
                        type="button"
                        className=""
                        onClick={() => setShowSignin(!showSignin)}
                        style={{ margin: 'auto' }}
                    >
                        Click here to {showSignin ? 'signup' : 'signin'}.
                    </button>
                </div>
            </div>
        </div>
    );
}
