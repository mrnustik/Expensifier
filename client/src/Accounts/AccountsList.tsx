import { Fab } from '@mui/material';
import { Add } from '@mui/icons-material';
import { useQuery } from '@tanstack/react-query';
import React from 'react';
import { useNavigate } from 'react-router';
import { IAccountListItem } from './API/IAccountListItem';
import { AccountListItemCard } from './Components/AccountListItemCard';
import { loadAccounts } from './API/loadAccounts';

export const AccountList: React.FC = () => {
    const navigate = useNavigate();
    const { data, error, isLoading } = useQuery({
        queryKey: ['accounts'],
        queryFn: loadAccounts
    })

    if (isLoading) {
        return <h1>Loading</h1>
    }

    if (error) {
        return <h1>Error...</h1>
    }

    const onCreateClick = () => {
        navigate("/accounts/create")
    };

    return <>
        <h1>Account List</h1>
        { data?.map((account: IAccountListItem) => 
            <AccountListItemCard key={account.id} item={account}/>)
        }
        <Fab style={{position:'fixed', bottom: 40, right: 40}}
             onClick={onCreateClick}>
            <Add />
        </Fab>
    </>;
};