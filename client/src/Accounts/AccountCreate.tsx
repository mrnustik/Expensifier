import { Button, Grid, Stack, TextField } from "@mui/material";
import { useMutation } from "@tanstack/react-query";
import React, { useState } from "react";
import { useNavigate } from "react-router";
import { createAccount } from "./API/createAccount";

export const AccountCreate: React.FC = () => {
    const [name, setName] = useState<string>('');
    const navigate = useNavigate();
    const mutation = useMutation({
        mutationFn: createAccount,
        onSuccess: () => {
            navigate('/accounts/');
        }
    })

    const onTextChange = (e: any) => setName(e.target.value);

    return <>
        <h1>Create account</h1>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate({ name: name }); }}>
            <Stack spacing={2}>
                <TextField autoFocus fullWidth label="Name" value={name} onChange={onTextChange} />
                <Button type="submit" variant="contained">
                    Create
                </Button>
            </Stack>
        </form>
    </>
};