import { useQuery } from '@tanstack/react-query'
import ky from 'ky';

export default function App() {
    const { isLoading, error, data } = useQuery({
        queryKey: ['data'],
        queryFn: () => ky.get('https://localhost:7005/').text(),
    })

    if (isLoading) return 'Loading...'

    if (error) return 'An error has occurred: ' + (error instanceof Error ? error.message : 'unknown')
  
    return (<>
        {data}
    </>)
}