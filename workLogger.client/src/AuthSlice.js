import { createSlice } from '@reduxjs/toolkit'
import { loginWithGoogle } from './authThunks';

const initialState = {
    loading: false,
    userInfo: 'initial val',
    userJwtToken: null,
    userGoogleToken: null,
    error: null,
    success: false
}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.loading = false
            state.userInfo = null
            state.userJwtToken = null
            state.userGoogleToken = null
            state.error = null
        },
        setCredentials: (state, { payload }) => {
            const userInfo = {
                name: payload.name,
                roles: payload.roles
            };
            
            state.userInfo = userInfo;
        },
        setGoogleToken: (state, { payload }) => {
            state.userGoogleToken = payload
        },
        setJwtToken: (state, { payload }) => {
            state.userJwtToken = payload
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(loginWithGoogle.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(loginWithGoogle.fulfilled, (state, action) => {
                state.loading = false;
                state.userJwtToken = action.payload.token;
                authSlice.caseReducers.setJwtToken(state, { payload: action.payload.token });
                authSlice.caseReducers.setCredentials(state, { payload: action.payload });
            })
            .addCase(loginWithGoogle.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload;
            });
    }
})

export const { logout, setGoogleToken } = authSlice.actions
export default authSlice.reducer
