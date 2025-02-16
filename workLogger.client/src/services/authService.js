import {apiClient, anonymousApiClient} from './apiClient';

export const login = async (googleToken) => {
    return anonymousApiClient('/OauthLogin/google-login', {
        method: 'POST',
        body: JSON.stringify({ token: googleToken }),
    });
};

// export const getUserInfo = async () => {
//     const response = await apiClient.get('/user/info');
//     return response.data;
// };