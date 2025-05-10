import {apiClient} from './apiClient';

export const getHolidaysList = async () => {
    return apiClient('/Holidays/list', {
        method: 'GET'
    });
};

export const addHoliday = async (holidayDate, holidayName) => {
    const data = {
        name: holidayName,
        date: holidayDate
    };
    
    return apiClient('/Holidays/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data,
    });
};

export const importHolidays = async () => {
    return apiClient('/Holidays/importHolidays', {
        method: 'POST'
    });
};

export const removeHoliday = async (holidayDate) => {
    return apiClient('/Holidays/remove', {
        method: 'POST',
        body: holidayDate,
    });
};
