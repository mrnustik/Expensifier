import { Box, AppBar, Toolbar, Typography, Container } from "@mui/material";
import React from "react";
import { Outlet } from "react-router";

export const Root: React.FC = () => {
    return <>
        <div>
            <Box sx={{ display: 'flex' }}>
                <AppBar position='absolute'>
                    <Toolbar>
                        <Typography>Expensifier</Typography>
                    </Toolbar>
                </AppBar>
                <Box component="main"
                    sx={{
                        flexGrow: 1,
                        height: '100vh',
                        overflow: 'auto',
                    }}>
                    <Toolbar />
                    <Container sx={{ mt: 4, mb: 4, height: '100vh' }}>
                        <Outlet />
                    </Container>
                </Box>
            </Box>
        </div>
    </>
}