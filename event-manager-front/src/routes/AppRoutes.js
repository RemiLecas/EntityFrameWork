import { Routes, Route } from 'react-router-dom';
import Home from '../components/Home';
import EventList from '../components/events/EventList';
import EventDetail from '../components/events/EventDetails';
import EventForm from '../components/events/eventForm';

const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<Home/>} />
    <Route path="/events" element={<EventList />} />
    <Route path="/events/:id" element={<EventDetail />} />
    <Route path="/events/new" element={<EventForm />} />
    <Route path="/events/:id/edit" element={<EventForm />} />
    <Route path="/events/create" element={<EventForm  />} />
    <Route path="/events/edit/:id" element={<EventForm />} />
  </Routes>
);

export default AppRoutes;
