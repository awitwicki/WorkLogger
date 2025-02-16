import {apiClient} from './apiClient';

export const getEmployeeSettings = async () => {
    return apiClient('/EmployeeSettings/get', {
        method: 'GET'
    });
};

// export const getUserInfo = async () => {
//     const response = await apiClient.get('/user/info');
//     return response.data;
// };