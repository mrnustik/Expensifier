import {Button, Card, CardActions, CardHeader, IconButton} from "@mui/material";
import {IAccountListItem} from "../API/loadAccounts";
import React from "react";
import {Delete} from "@mui/icons-material";


interface Props {
    onDeleteClick: (id: string) => void;
    item: IAccountListItem;

}

export const AccountListItemCard : React.FC<Props> = (props) => {
    return (
        <Card sx={{minWidth: 275}}>
            <CardHeader
                title={props.item.name}
                action={
                    <IconButton onClick={() => props.onDeleteClick(props.item.id)}>
                        <Delete />
                    </IconButton>
                }/>
            <CardActions>
                <Button size="small">
                    Add expense
                </Button>
                <Button size="small">
                    Add income
                </Button>
            </CardActions>
        </Card>
    )
};