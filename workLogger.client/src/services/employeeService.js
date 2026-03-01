import { apiClient } from './apiClient';

export const getEmployees = async () => {
    return apiClient('/UserManage/list', { method: 'GET' });
};

export const removeUser = async (userId) => {
    return apiClient(`/UserManage/RemoveUser?userId=${userId}`, { method: 'POST' });
};

export const addRoleToUser = async (userId, roleName) => {
    return apiClient(`/UserManage/AddRoleToUser?userId=${userId}&roleName=${roleName}`, { method: 'POST' });
};

export const cleanUserRoles = async (userId) => {
    return apiClient(`/UserManage/CleanUserRoles?userId=${userId}`, { method: 'POST' });
};
