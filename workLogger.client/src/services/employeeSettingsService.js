import { apiClient } from './apiClient';

export const getEmployeeSettings = async () => {
    return apiClient('/EmployeeSettings/get', { method: 'GET' });
};

export const saveEmployeeSettings = async (settings) => {
    return apiClient('/EmployeeSettings/save', {
        method: 'POST',
        body: settings
    });
};

export const getEmployeeSettingsById = async (userId) => {
    return apiClient(`/EmployeeSettings/get/${userId}`, { method: 'GET' });
};

export const saveEmployeeSettingsById = async (userId, settings) => {
    return apiClient(`/EmployeeSettings/save/${userId}`, {
        method: 'POST',
        body: settings
    });
};
