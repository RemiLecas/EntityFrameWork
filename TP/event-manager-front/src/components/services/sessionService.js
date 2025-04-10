import axios from 'axios';

const API_URL = 'http://localhost:5257/api/Sessions';

// Fonction pour obtenir une session par son ID
export const getSessionById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Erreur lors de la récupération de la session avec l'ID ${id}`, error);
    throw error;
  }
};

export const getSessions = async () => {
  try {
    const response = await axios.get(`${API_URL}`);
    return response.data;
  } catch (error) {
    console.error('Erreur lors de la récupération des sessions', error);
    throw error;
  }
};

