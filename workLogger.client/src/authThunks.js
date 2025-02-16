import { createAsyncThunk } from '@reduxjs/toolkit';
import { login } from './services/authService'
export const loginWithGoogle = createAsyncThunk(
    'auth/loginWithGoogle',
    async (googleToken, { rejectWithValue }) => {
        try {
            const response = await login(googleToken);
            
            if (!response.ok) {
                console.log(`HTTP error! Status: ${response.status}`);
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            return response.json();
         
            // return data;
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);
