import axios from 'axios';

const API_URL = 'http://localhost:5257/api/Sessions';

// Fonction pour créer une session
export const createSession = async (sessionData) => {
  try {
    const response = await axios.post(`${API_URL}`, sessionData);
    return response.data;
  } catch (error) {
    console.error('Erreur lors de la création de la session', error);
    throw error;
  }
};

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
