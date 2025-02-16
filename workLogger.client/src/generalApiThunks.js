import { createAsyncThunk } from '@reduxjs/toolkit';
import { consts } from './consts'

export const fetchAdminData = createAsyncThunk(
    'general/fetchAdminData',
    async (_, { getState, rejectWithValue }) => {
        try {
            const token = getState().auth.userJwtToken; // Отримуємо JWT токен з Redux

            const headers = {
                'Content-Type': 'application/json',
                Authorization: token ? `Bearer ${token}` : '',
            };

            const response = await fetch(`${consts.baseUrl}/test-auth/admin-endpoint`, {
                method: 'GET',
                headers,
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);

export const fetchUserData = createAsyncThunk(
    'general/fetchUserData',
    async (_, { getState, rejectWithValue }) => {
        try {
            const token = getState().auth.userJwtToken; // Отримуємо JWT токен з Redux

            const headers = {
                'Content-Type': 'application/json',
                Authorization: token ? `Bearer ${token}` : '',
            };

            const response = await fetch(`${consts.baseUrl}/test-auth/user-endpoint`, {
                method: 'GET',
                headers,
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);
