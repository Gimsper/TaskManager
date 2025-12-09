import axios from 'axios';

const API_URL = 'https://localhost:44391/api';
const AUTH_USER = '9gk7sPAj9hE=';
const AUTH_PWD = 'NsX8xEav35+BvurRn3x2bANt7lnq2RJ6odp/zr3HQ+k=';
let AUTH_TOKEN = '';

export type ResultOperationVoid = {
    isSuccess: boolean;
    message: string;
}

export type ResultOperation<T> = {
    data: T | null;
    isSuccess: boolean;
    message: string;
};

const getToken = async () => {
    let result = "";
    await axios.post(`${API_URL}/Token/Authentication`, {
        User: AUTH_USER,
        Password: AUTH_PWD
    }).then(response => {
        result = response.data.token;
        AUTH_TOKEN = result;
    }).catch(error => {
        console.error('Error fetching token:', error);
    });
    return result;
};

const createClient = async () => {
    if (!AUTH_TOKEN) {
        await getToken();
    }
    return axios.create({
        baseURL: API_URL,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${AUTH_TOKEN}`
        }
    });
};

export const client = await createClient();