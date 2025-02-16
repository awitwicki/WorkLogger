import {apiClient} from './apiClient';

export const getEmployees = async () => {
    return apiClient('/UserManage/list', {
        method: 'GET'
    });
};
