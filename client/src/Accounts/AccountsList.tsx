import { Fab } from '@mui/material';
import { Add } from '@mui/icons-material';
import {useMutation, useQuery, useQueryClient} from '@tanstack/react-query';
import React from 'react';
import { useNavigate } from 'react-router';
import { AccountListItemCard } from './Components/AccountListItemCard';
import { IAccountListItem, loadAccounts } from './API/loadAccounts';
import {deleteAccount} from "./API/deleteAccount";

export const AccountList: React.FC = () => {
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const { data, error, isLoading } = useQuery({
        queryKey: ['accounts'],
        queryFn: loadAccounts
    })

    const deleteMutation = useMutation({
        mutationFn: deleteAccount,
        mutationKey: ['accounts'],
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ['accounts'] });
        }
    })

    const deleteItem = (id: string) => {
        deleteMutation.mutate(id);
    };

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
            <AccountListItemCard
                key={account.id}
                item={account}
                onDeleteClick={deleteItem}/>)
        }
        <Fab style={{position:'fixed', bottom: 40, right: 40}}
             onClick={onCreateClick}>
            <Add />
        </Fab>
    </>;
};