import { useEffect, useState } from 'react';
import eventService from '../services/eventService';
import categoryService from '../services/categoriesService';
import { Link, useNavigate } from 'react-router-dom';
import './eventList.css';

const EventList = () => {
  const [events, setEvents] = useState([]);
  const [categories, setCategories] = useState([]);
  const [filters, setFilters] = useState({
    startDate: '',
    endDate: '',
    locationId: '',
    categoryId: '',
    status: '',
  });
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const navigate = useNavigate();


  // Charger les catégories
  const fetchCategories = async () => {
    const categories = await categoryService.getCategories();
    setCategories(categories);
  };

  const fetchEvents = async () => {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: '5',
    });

    if (filters.categoryId) {
        params.append('category', filters.categoryId);
    }
  
    // Si des filtres supplémentaires (dates) sont présents, on les ajoute à l'URL
    if (filters.startDate) params.append('startDate', filters.startDate);
    if (filters.endDate) params.append('endDate', filters.endDate);
  
    try {
      const response = await eventService.getEvents(params);
      console.log("Fetched events: ", response);
      setEvents(response.events);
      setTotalPages(response.totalPages || 1);
    } catch (error) {
      console.error("Error fetching events: ", error);
    }
  };

  // Charger les événements au premier rendu sans filtre
  useEffect(() => {
    fetchEvents(); // Charger les événements lorsque le composant est monté
  }, [filters, page]);  // Ajoute filters ici pour recharger avec les nouveaux filtres

  // Charger les catégories lors du premier rendu
  useEffect(() => {
    fetchCategories(); 
  }, []);

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters((prevFilters) => ({
      ...prevFilters,
      [name]: value,
    }));
  };

  const handlePageChange = (newPage) => {
    setPage(newPage);
  };

  const handleSearch = () => {
    setPage(1);
    fetchEvents();
  };

  const handleCreate = () => {
    navigate('/events/create');
  };

  return (
    <div className="event-list">
      <h2>Liste des événements</h2>

      {/* Filtres */}
      <div className="filters">
        <select name="categoryId" value={filters.categoryId} onChange={handleFilterChange}>
          <option value="">Choisir une catégorie</option>
          {categories?.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>

        <input
          type="date"
          name="startDate"
          value={filters.startDate}
          onChange={handleFilterChange}
        />
        <input
          type="date"
          name="endDate"
          value={filters.endDate}
          onChange={handleFilterChange}
        />

        <button onClick={handleSearch}>Rechercher</button>

        <button onClick={handleCreate}>Créer</button>

      </div>

      {/* Liste des événements */}
      <ul>
        {events?.length > 0 ? (
          events.map((event) => (
            <li key={event.id}>
              <Link to={`/events/${event.id}`}>
                {event.title} - {event.category ? event.category.name : 'Sans catégorie'}
              </Link>
            </li>
          ))
        ) : (
          <p>Aucun événement trouvé.</p>
        )}
      </ul>

      {/* Pagination */}
      <div className="pagination">
        {page > 1 && (
          <button onClick={() => handlePageChange(page - 1)}>Précédent</button>
        )}
        <span>Page {page} sur {totalPages}</span>
        {page < totalPages && (
          <button onClick={() => handlePageChange(page + 1)}>Suivant</button>
        )}
      </div>
    </div>
  );
};

export default EventList;
