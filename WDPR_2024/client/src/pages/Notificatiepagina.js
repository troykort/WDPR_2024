import React, { useState, useEffect } from "react";
import axios from "axios";
import "./Notificatiepagina.css";

const NotificatiePagina = () => {
    const [notificaties, setNotificaties] = useState([]);
    const [selectedNotificatie, setSelectedNotificatie] = useState(null);
    const [modalVisible, setModalVisible] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filter, setFilter] = useState('alle');
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    useEffect(() => {
        fetchNotificaties();
    }, [page]);

    const fetchNotificaties = async () => {
        setLoading(true);
        setError(null);
        try {
            const token = localStorage.getItem("token");
            const userid = localStorage.getItem('Id');
            const response = await axios.get(`http://localhost:5000/api/notificaties/${userid}?page=${page}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            if (response.data.length === 0) {
                setHasMore(false);
            } else {
                const sortedNotificaties = response.data.sort((a, b) => new Date(b.verzondenOp) - new Date(a.verzondenOp));
                setNotificaties(prev => [...prev, ...sortedNotificaties]);
            }
        } catch (error) {
            setError("Er is een fout opgetreden bij het ophalen van de notificaties.");
            console.error("Error fetching notificaties:", error);
        } finally {
            setLoading(false);
        }
    };

    const markAsRead = async (id) => {
        try {
            const token = localStorage.getItem("token");
            await axios.put(`http://localhost:5000/api/notificaties/${id}/gelezen`, {}, {
                headers: { Authorization: `Bearer ${token}` },
            });
            setNotificaties(prev => prev.map(notificatie =>
                notificatie.notificatieID === id ? { ...notificatie, gelezen: true } : notificatie
            ));
        } catch (error) {
            console.error("Error marking notificatie as read:", error);
        }
    };

    const handleNotificatieClick = (notificatie) => {
        setSelectedNotificatie(notificatie);
        setModalVisible(true);
        if (!notificatie.gelezen) {
            markAsRead(notificatie.notificatieID);
        }
    };

    const handleCloseModal = () => {
        setSelectedNotificatie(null);
        setModalVisible(false);
    };

    const handleLoadMore = () => {
        setPage(prev => prev + 1);
    };

    const filteredNotificaties = notificaties.filter(notificatie => {
        if (filter === 'gelezen') return notificatie.gelezen;
        if (filter === 'ongelezen') return !notificatie.gelezen;
        return true;
    });

    return (
        <div className="notificatie-pagina-container">
            <h2>Notificaties</h2>
            <select value={filter} onChange={(e) => setFilter(e.target.value)}>
                <option value="alle">Alle</option>
                <option value="gelezen">Gelezen</option>
                <option value="ongelezen">Ongelezen</option>
            </select>
            {loading && <p>Laden...</p>}
            {error && <p className="error-message">{error}</p>}
            <ul className="notificatie-lijst" role="list">
                {filteredNotificaties.map((notificatie) => (
                    <li
                        key={notificatie.notificatieID}
                        className={`notificatie-item ${notificatie.gelezen ? "gelezen" : "ongelezen"}`}
                        onClick={() => handleNotificatieClick(notificatie)}
                        role="button"
                        tabIndex="0"
                        onKeyPress={(e) => { if (e.key === 'Enter') handleNotificatieClick(notificatie); }}
                        aria-pressed={notificatie.gelezen}
                    >
                        <h3>{notificatie.titel}</h3>
                        <p>{notificatie.bericht.substring(0, 50)}...</p>
                        <span>{new Date(notificatie.verzondenOp).toLocaleString()}</span>
                    </li>
                ))}
            </ul>
            {hasMore && (
                <button onClick={handleLoadMore} className="load-more-button">
                    Meer laden
                </button>
            )}
            {modalVisible && selectedNotificatie && (
                <div className="notificatie-modal visible" role="dialog" aria-labelledby="modal-title" aria-describedby="modal-description">
                    <div className="notificatie-modal-content">
                        <h3 id="modal-title">{selectedNotificatie.titel}</h3>
                        <p id="modal-description">{selectedNotificatie.bericht}</p>
                        <p>
                            <strong>Verzonden op:</strong>{" "}
                            {new Date(selectedNotificatie.verzondenOp).toLocaleString()}
                        </p>
                        <button onClick={handleCloseModal} aria-label="Sluiten">Sluiten</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default NotificatiePagina;





