import { apiClient } from './apiClient';

// User endpoints
export const getMyMonth = async (date) => {
    return apiClient(`/MonthDay/MyMonth?date=${date}`, { method: 'GET' });
};

export const saveMyMonth = async (days, date) => {
    return apiClient(`/MonthDay/SaveMyMonth?date=${date}`, {
        method: 'POST',
        body: days
    });
};

export const getMyMonths = async () => {
    return apiClient('/MonthDay/MyMonths', { method: 'GET' });
};

export const exportMyPdf = async (date) => {
    return apiClient(`/MonthDay/ExportMyPdf?date=${date}`, { method: 'GET' });
};

// Admin endpoints
export const getMonth = async (date, userId) => {
    return apiClient(`/MonthDay/GetMonth?date=${date}&userId=${userId}`, { method: 'GET' });
};

export const getUserMonths = async (userId) => {
    return apiClient(`/MonthDay/GetUserMonths?userId=${userId}`, { method: 'GET' });
};

export const saveMonth = async (days, date, userId) => {
    return apiClient(`/MonthDay/SaveMonth?date=${date}&userId=${userId}`, {
        method: 'POST',
        body: days
    });
};

export const exportPdf = async (date, userId) => {
    return apiClient(`/MonthDay/ExportPdf?date=${date}&userId=${userId}`, { method: 'GET' });
};
