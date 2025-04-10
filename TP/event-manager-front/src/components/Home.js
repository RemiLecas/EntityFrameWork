import React from 'react';
import './Home.css';
import EventList from './events/EventList.js';

function Home() {
  return (
    <div className="home">
      <h1>Bienvenue sur la page d'accueil</h1>
      <EventList />
    </div>
  );
}

export default Home;
