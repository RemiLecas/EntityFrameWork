import axios from 'axios';

const API_URL = 'http://localhost:5257/api/Events';

export const getEvents = async (params) => {
  const res = await axios.get(`${API_URL}?${params.toString()}`);
  return res.data;
};

export const getEvent = async (id) => {
  const res = await axios.get(`${API_URL}/${id}`);
  return res.data;
};

export const createEvent = async (event) => {
  const res = await axios.post(API_URL, event);
  return res.data;
};

export const updateEvent = async (id, event) => {
  const res = await axios.put(`${API_URL}/${id}`, event);
  return res.data;
};

export const deleteEvent = async (id) => {
  await axios.delete(`${API_URL}/${id}`);
};


export default {
    getEvents,
    getEvent,
    createEvent,
    updateEvent,
    deleteEvent,
  };