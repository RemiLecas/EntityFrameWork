import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { getCategories } from '../services/categoryService';
import { getLocations } from '../services/locationService';
import { getParticipants } from '../services/participantService';
import { getSpeakers } from '../services/speakerService';
import { getRooms } from '../services/roomService';
import { createSession, getSessionById } from '../services/sessionService';

import eventService from '../services/eventService';
import './eventForm.css';

const EventForm = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [event, setEvent] = useState({
    title: '',
    description: '',
    startDate: '',
    endDate: '',
    status: 0, // "Planned"
    categoryId: '',
    locationId: '',
    roomId: '',
    participantIds: [],
    speakerIds: [],
  });
  const [categories, setCategories] = useState([]);
  const [locations, setLocations] = useState([]);
  const [participants, setParticipants] = useState([]);
  const [speakers, setSpeakers] = useState([]);
  const [error, setError] = useState('');
  const [filteredRooms, setFilteredRooms] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [sessions, setSessions] = useState([]);  // Assurez-vous que sessions est bien défini ici
  const [sessionId, setSessionId] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [categoriesData, locationsData, participantsData, speakersData, roomsData, sessionsData] = await Promise.all([
          getCategories(),
          getLocations(),
          getParticipants(),
          getSpeakers(),
          getRooms(),
        ]);

        setCategories(categoriesData);
        setLocations(locationsData);
        setParticipants(participantsData);
        setSpeakers(speakersData);
        setRooms(roomsData);
        setSessions(sessionsData); // L'ajout des sessions ici

        if (id) {
          const eventData = await eventService.getEvent(id);
          setEvent(eventData);
          if (eventData.locationId) {
            filterRoomsByLocation(eventData.locationId);
          }

          if (eventData.sessionIds && eventData.sessionIds.length > 0) {
            const sessionId = eventData.sessionIds[0];
            const sessionData = await getSessionById(sessionId);
          }
        }

        
      } catch (error) {
        setError('Erreur lors du chargement des données');
      }
    };

    fetchData();
  }, [id]);

  const handleLocationChange = (e) => {
    const selectedLocationId = e.target.value;
    setEvent({
      ...event,
      locationId: selectedLocationId,
    });
    filterRoomsByLocation(selectedLocationId);
  };

  const filterRoomsByLocation = (selectedLocationId) => {
    const selectedLocation = locations.find(location => location.id === Number(selectedLocationId));
    setFilteredRooms(selectedLocation.rooms);
  };
  
  const handleChange = (e) => {
    setEvent({ ...event, [e.target.name]: e.target.value });
  };

  const handleSelectChange = (e, field) => {
    setEvent({
      ...event,
      [field]: [...e.target.selectedOptions].map(option => option.value),
    });
  };

  const handleCreateSession = async () => {
    const newSession = {
      title: event.title,  // Utilisez les données de l'événement pour créer la session
      description: event.description,
      startDate: event.startDate,
      endDate: event.endDate,
    };

    try {
      const createdSession = await createSession(newSession);
      setSessionId(createdSession.id);
      alert('Session créée avec succès!');
    } catch (error) {
      console.error('Erreur lors de la création de la session', error);
      setError('Erreur lors de la création de la session');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!sessionId) {
      await handleCreateSession(); // Créer la session si elle n'existe pas
    }

    const eventData = {
      title: event.title,
      description: event.description,
      startDate: event.startDate,
      endDate: event.endDate,
      status: event.status,
      categoryId: event.categoryId,
      locationId: event.locationId,
      roomId: event.roomId,
      participantIds: event.participantIds,
      speakerIds: event.speakerIds,
      sessionIds: sessionId ? [sessionId] : [], // Assurez-vous d'envoyer l'ID de la session
    };

    try {
      if (id) {
        await eventService.updateEvent(id, eventData);
        alert('Événement mis à jour avec succès!');
      } else {
        await eventService.createEvent(eventData);
        alert('Événement créé avec succès!');
      }
      setEvent({
        title: '',
        description: '',
        startDate: '',
        endDate: '',
        status: 0,
        categoryId: '',
        locationId: '',
        roomId: '',
        participantIds: [],
        speakerIds: [],
      });
      setSessionId(null);
      navigate('/events');
    } catch (error) {
      setError('Erreur lors de la création ou mise à jour de l\'événement');
    }
  };

  return (
    <div className="form-container">
      <h2>{id ? 'Modifier l\'événement' : 'Ajouter un événement'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Titre</label>
          <input
            type="text"
            name="title"
            value={event.title}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Description</label>
          <textarea
            name="description"
            value={event.description}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label>Date de début</label>
          <input
            type="datetime-local"
            name="startDate"
            value={event.startDate}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Date de fin</label>
          <input
            type="datetime-local"
            name="endDate"
            value={event.endDate}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Statut</label>
          <select
            name="status"
            value={event.status}
            onChange={handleChange}
            required
          >
            <option value={0}>Planned</option>
            <option value={1}>Canceled</option>
            <option value={2}>Completed</option>
          </select>
        </div>
        <div className="form-group">
          <label>Catégorie</label>
          <select
            name="categoryId"
            value={event.categoryId}
            onChange={handleChange}
            required
          >
            <option value="">Sélectionner une catégorie</option>
            {categories.map((category) => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Lieu</label>
          <select
            name="locationId"
            value={event.locationId}
            onChange={handleLocationChange}
            required
          >
            <option value="">Sélectionner un lieu</option>
            {locations.map((location) => (
              <option key={location.id} value={location.id}>
                {location.name}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Salle</label>
          <select
            name="roomId"
            value={event.roomId}
            onChange={handleChange}
            required
          >
            <option value="">Sélectionner une salle</option>
            {filteredRooms?.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Participants</label>
          <select
            multiple
            name="participantIds"
            value={event.participantIds}
            onChange={(e) => handleSelectChange(e, 'participantIds')}
          >
            {participants.map((participant) => (
              <option key={participant.id} value={participant.id}>
                {participant.firstName} {participant.lastName}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Speakers</label>
          <select
            multiple
            name="speakerIds"
            value={event.speakerIds}
            onChange={(e) => handleSelectChange(e, 'speakerIds')}
          >
            {speakers.map((speaker) => (
              <option key={speaker.id} value={speaker.id}>
                {speaker.firstName} {speaker.lastName}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
            <label>Sessions</label>
            <select
                multiple
                name="sessionIds"
                value={event.sessionIds || []}  // Assurez-vous que `sessionIds` est un tableau dans `event`
                onChange={(e) => handleSelectChange(e, 'sessionIds')}
            >
                {sessions?.map((session) => (
                <option key={session.id} value={session.id}>
                    {session.title}
                </option>
                ))}
            </select>
        </div>

        <div>
          {error && <p className="error">{error}</p>}
        </div>
        <div>
          <button type="submit">{id ? 'Mettre à jour' : 'Ajouter l\'événement'}</button>
        </div>
      </form>
    </div>
  );
};

export default EventForm;
