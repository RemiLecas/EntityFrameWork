import axios from 'axios';

const API_URL = 'http://localhost:5257/api/Rooms';


export const getRooms = async () => {
    
  try {
    const response = await axios.get(API_URL);
    return response.data;
  } catch (error) {
    throw new Error('Erreur lors de la récupération des catégories');
  }
};
