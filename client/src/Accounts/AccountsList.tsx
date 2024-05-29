import { useQuery } from '@tanstack/react-query';
import React from 'react';

async function getAccountsList() {
    const response = await fetch('/api/accounts');
    return response.json();
}

export const AccountList: React.FC = () => {
    const {data, error, isLoading} = useQuery({
        queryKey: ['accounts'],
        queryFn: getAccountsList
    })

    if(isLoading) {
        return <h1>Loading</h1>
    }

    if(error) {
        return <h1>Error...</h1>
    }

    return <>
        <h1>Account List</h1>
        <ul>
            {data.map((account: any) => <li key={account.id}>{account.name}</li>)}
        </ul>
    </>;
};