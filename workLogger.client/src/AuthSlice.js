import { createSlice } from '@reduxjs/toolkit'
import { loginWithGoogle, checkUserStatus } from './authThunks';


const initialState = {
    loading: false,
    userInfo: 'initial val',
    userJwtToken: null,
    userGoogleToken: null,
    error: false,
    success: false
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.loading = false;
            state.userInfo = null;
            state.userJwtToken = null;
            state.userGoogleToken = null;
            state.error = false;
        },
        setCredentials: (state, { payload }) => {
            const userInfo = {
                name: payload.name,
                roles: payload.roles
            };
            
            state.userInfo = userInfo;
        },
        setGoogleToken: (state, { payload }) => {
            state.userGoogleToken = payload;
        },
        setJwtToken: (state, { payload }) => {
            state.userJwtToken = payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(loginWithGoogle.pending, (state) => {
                state.loading = true;
            })
            .addCase(loginWithGoogle.fulfilled, (state, action) => {
                state.loading = false;
                state.userJwtToken = action.payload.token;
                authSlice.caseReducers.setJwtToken(state, { payload: action.payload.token });
                authSlice.caseReducers.setCredentials(state, { payload: action.payload });
            })
            .addCase(loginWithGoogle.rejected, (state, action) => {
                console.log('loginWithGoogle.rejected', action);
                state.loading = false;
                state.error = true;
            })
            .addCase(checkUserStatus.pending, (state) => {
                console.log('checkUserStatus.pending');
                state.loading = true;
            })
            .addCase(checkUserStatus.fulfilled, (state, action) => {
                console.log('checkUserStatus.fulfilled', action);
                state.loading = false;
                state.error = false;
                authSlice.caseReducers.setCredentials(state, { payload: action.payload });
            })
            .addCase(checkUserStatus.rejected, (state, action) => {
                console.log('checkUserStatus.rejected', action);
                state.loading = false;
                state.error = true;
                authSlice.caseReducers.logout(state);
            });
    }
});

export const { logout, setGoogleToken } = authSlice.actions;
export default authSlice.reducer;