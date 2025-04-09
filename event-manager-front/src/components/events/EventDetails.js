import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import eventService from '../services/eventService';
import './eventDetails.css';

const EventDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [event, setEvent] = useState(null);

  useEffect(() => {
    const fetchEvent = async () => {
      const data = await eventService.getEvent(id);
      setEvent(data);
    };

    fetchEvent();
  }, [id]);

  const handleDelete = async () => {
    await eventService.deleteEvent(id);
    navigate('/');
  };

  if (!event) return <div>Chargement...</div>;

  return (
    <div className="event-detail">
      <h2>{event.title}</h2>
      <p>{event.description}</p>

      {/* Catégorie */}
      <div className="category">
        <strong>Catégorie : </strong>
        {event.category ? event.category.name : 'Non spécifiée'}
      </div>

      {/* Dates */}
      <div className="dates">
        <strong>Date de début : </strong>{new Date(event.startDate).toLocaleString()}<br />
        <strong>Date de fin : </strong>{new Date(event.endDate).toLocaleString()}
      </div>

      {/* Localisation */}
      <div className="location">
        <strong>Lieu : </strong>
        {event.location ? `${event.location.name}, ${event.location.city}, ${event.location.country}` : 'Non spécifié'}
      </div>

      {/* Sessions */}
      <div className="sessions">
        <strong>Sessions :</strong>
        {event.sessions && event.sessions.length > 0 ? (
          <ul>
            {event.sessions.map((session) => (
              <li key={session.id}>
                {session.title} (Salle: {session.roomName})<br />
                {session.speakers && session.speakers.length > 0 && (
                  <span>Intervenant: {session.speakers[0].firstName} {session.speakers[0].lastName}</span>
                )}
              </li>
            ))}
          </ul>
        ) : (
          <p>Aucune session disponible</p>
        )}
      </div>

      {/* Participants */}
      <div className="participants">
        <strong>Participants :</strong>
        {event.participants && event.participants.length > 0 ? (
          <ul>
            {event.participants.map((participant) => (
              <li key={participant.id}>
                {participant.firstName} {participant.lastName} ({participant.email})
              </li>
            ))}
          </ul>
        ) : (
          <p>Aucun participant inscrit</p>
        )}
      </div>

      {/* Bouton pour supprimer */}
      <button className="delete-btn" onClick={handleDelete}>Supprimer l'événement</button>
    </div>
  );
};

export default EventDetail;
