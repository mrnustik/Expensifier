import { Button, Card, CardActions, CardContent, Typography } from "@mui/material";
import { IAccountListItem } from "../API/loadAccounts";


interface Props {
    item: IAccountListItem;
}

export const AccountListItemCard : React.FC<Props> = (props) => {
    return (
        <Card sx={{minWidth: 275}}>
            <CardContent>
                <Typography variant="h5">
                    {props.item.name}
                </Typography>
            </CardContent>
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