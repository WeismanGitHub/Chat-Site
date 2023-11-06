// import { useQuery } from '@tanstack/react-query'
import { Form } from 'react-bootstrap';
// import ky from 'ky';

export default function Auth() {
    // const { isLoading, error, data } = useQuery({
    //     queryKey: ['data'],
    //     queryFn: () => ky.get('https://localhost:7005/').text(),
    // })

    // if (error) return 'An error has occurred: ' + (error instanceof Error ? error.message : 'unknown')

    return (
        <>
            <Form>
                <div className="mb-3">
                    <label htmlFor="exampleInputEmail1" className="form-label">
                        Email address
                    </label>
                    <input
                        type="email"
                        className="form-control"
                        id="exampleInputEmail1"
                        aria-describedby="emailHelp"
                    />
                </div>
            </Form>
        </>
    );
}
