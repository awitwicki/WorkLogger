import { consts } from '../consts'

let getState = null; // Ініціалізуємо змінну для збереження getState

export const setGetState = (fn) => {
    getState = fn; // Передаємо функцію отримання стейту
};

// export const getToken = () => {
//     const state = store.getState();
//     return state.auth.userJwtToken;
// };
//


const anonymousApiClient = async (endpoint, options = {}) => {
    const baseUrl = consts.baseUrl;

    const defaultHeaders = {
        'Content-Type': 'application/json',
    };

    // Об'єднуємо заголовки
    const headers = { ...defaultHeaders, ...options.headers };

    const config = {
        ...options,
        headers,
    };

   return fetch(`${baseUrl}${endpoint}`, config);
};

const apiClient = (endpoint, { method = 'GET', body = null, headers = {} } = {}) => {
    const baseUrl = consts.baseUrl

    const token = getState ? getState().auth.userJwtToken : null;
    
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const config = {
        method,
        headers: {
            'Content-Type': 'application/json',
            ...headers,
        },
        body: body ? JSON.stringify(body) : null,
    };

    return fetch(`${baseUrl}${endpoint}`, config);
};

export {apiClient, anonymousApiClient};
