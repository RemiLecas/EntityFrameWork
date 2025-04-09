import axios from 'axios';

export const getSpeakers = async () => {
const API_URL = 'http://localhost:5257/api/Speaker/speakers';

  try {
    const response = await axios.get(API_URL);
    return response.data;
  } catch (error) {
    throw new Error('Erreur lors de la récupération des speakers');
  }
};
