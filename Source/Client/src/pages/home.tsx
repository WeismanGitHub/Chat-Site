import Navbar from "../components/navbar"
// import { useQuery } from '@tanstack/react-query';
// import ky from 'ky';

// type friend = {
//     ID: string;
//     DisplayName: string;
//     CreatedAt: string;
// };

export default function Home() {
    return <Navbar />
    // const { isLoading, error, data } = useQuery({
    //     queryKey: ['data'],
    //     queryFn: (): Promise<friend[]> => ky.get('/api/friends').json(),
    // });

    // if (error) {
    //     throw error;
    // }

    // return (
    //     <div>
    //         {(isLoading == true ? [] : data!).map((friend) => {
    //             return (
    //                 <>
    //                     {friend.DisplayName}
    //                     {friend.CreatedAt}
    //                 </>
    //             );
    //         })}
    //     </div>
    // );
}
