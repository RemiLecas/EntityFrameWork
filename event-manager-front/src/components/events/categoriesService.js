import axios from 'axios';

const API_URL = 'http://localhost:5257/api';

export const getCategories = async () => {
    const response = await fetch(`${API_URL}/Categories`);
    const data = await response.json();
    return data;
};

export default {
    getCategories
};