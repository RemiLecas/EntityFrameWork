import axios from 'axios';

export const getLocations = async () => {
const API_URL = 'http://localhost:5257/api/Locations';

  try {
    const response = await axios.get(API_URL);
    return response.data;
  } catch (error) {
    throw new Error('Erreur lors de la récupération des lieux');
  }
};
