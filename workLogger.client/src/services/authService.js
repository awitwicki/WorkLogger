import {apiClient, anonymousApiClient} from './apiClient';

export const login = async (googleToken) => {
    return anonymousApiClient('/OauthLogin/google-login', {
        method: 'POST',
        body: JSON.stringify({ token: googleToken })
    });
};

export const getUserInfo = async () => {
    return apiClient('/getme', {
        method: 'GET',
    });
};
